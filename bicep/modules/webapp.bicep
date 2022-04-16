param location string
param appName string
param appSettings array = []

resource servicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: 'SP-${appName}'
  location: location
  sku: {
    name: contains(appName, 'MVC') ? 'S1' : 'F1'
  }
}

resource webApp 'Microsoft.Web/sites@2021-03-01' = {
  name: '${appName}-${uniqueString(resourceGroup().id)}'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: servicePlan.id
    siteConfig: {
      appSettings: appSettings
    }
  }
}
