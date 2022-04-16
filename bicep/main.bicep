param location string = resourceGroup().location
param appName string = 'AutoRapide'
param adminDbLogin string
@secure()
param adminDbPassword string

var storageConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'

var sqlServerName = 'autorapide-${uniqueString(resourceGroup().id)}-sqlserver'

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  kind: 'StorageV2'
  location: location
  name: '${toLower(appName)}storage'
  sku: {
    name: 'Standard_LRS'
  }

  resource blobService 'blobServices@2021-08-01' = {
    name: 'default'

    resource container 'containers@2021-08-01' = {
      name: 'images'
      properties: {
        publicAccess: 'Container'
      }
    }

  }

}

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2021-06-01-preview' = {
  name: 'acr${uniqueString(resourceGroup().id)}'
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: false
  }
}

resource redisCache 'Microsoft.Cache/redis@2021-06-01' = {
  name:'rediscache${uniqueString(resourceGroup().id)}'
  location: location
  properties: {
    sku: {
      name: 'Basic'
      capacity: 2
      family: 'C'
    }
  }
}

module sqlDatabases 'modules/sqlDatabases.bicep' = {
  name: 'sqldatabases'
  params: {
    adminDbLogin: adminDbLogin
    adminDbPassword: adminDbPassword
    dbNames: [
      'UsagerAPI'
      'VehiculesAPI'
      'CommandesAPI'
    ]
    location: location
    serverName: sqlServerName
  }
}

var appNames = [
  'UsagerAPI'
  'CommandesAPI'
  'FavorisAPI'
  '${appName}MVC'
]

module webApps 'modules/webapp.bicep' = [for name in appNames:{
  name: '${name}-webapp'
  params: {
    appName: name
    location: location
    appSettings: [
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Development'
      }
    ]
  }
}]

module functionApp 'modules/functionApp.bicep' = {
  name: 'fichiers-funcapp'
  params: {
    location: location
    appName: 'fichiers'
    containerName: storageAccount::blobService::container.name
    storageConnectionString: storageConnectionString
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' = {
  name: 'kv${uniqueString(resourceGroup().id)}'
  location: location
  properties: {
    sku: {
      name: 'standard'
      family: 'A'
    }
    tenantId: subscription().tenantId
    accessPolicies: []
  }
}
