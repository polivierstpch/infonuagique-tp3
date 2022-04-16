param location string
param serverName string
param adminDbLogin string
param adminDbPassword string
param dbNames array

resource sqlServer 'Microsoft.Sql/servers@2021-08-01-preview' = {
  location: location
  name: serverName
  properties: {
    administratorLogin: adminDbLogin
    administratorLoginPassword: adminDbPassword
  }

  resource firewallRule 'firewallRules@2021-08-01-preview' = {
    name: 'AllowAzureIPs'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '255.255.255.255'
    }
  }

  resource sqlDbs 'databases@2021-08-01-preview' = [for dbName in dbNames: {
    location: location
    name: '${dbName}Db'
    sku: {
      name: 'Basic'
      tier: 'Basic'
    }
  }]

}




