param (
    [String] $basePath = "C:\Kyazoonga\Kz"
)

Import-Module 'Carbon'

Uninstall-IisWebsite -Name 'Default Web Site'

Install-IisAppPool -Name "api.kz"
Install-IisWebsite -Name "api.kz.local" -PhysicalPath "$basePath\Kz.Api" -AppPoolName "api.kz" -Bindings @('http/*:80:api.kz.local','http/*:60080:')
New-NetFirewallRule -DisplayName "APIPort" -Profile Public -Direction Inbound -Action Allow -Protocol TCP -LocalPort 60080
Set-HostsEntry -IPAddress 127.0.0.1 -HostName 'api.kz.local' 

Install-IisAppPool -Name "configuration.kz"
Install-IisWebsite -Name "configuration.kz.local" -PhysicalPath "$basePath\Kz.Configuration.Api" -AppPoolName "configuration.kz" -Bindings @('http/*:80:configuration.kz.local','http/*:60081:')
New-NetFirewallRule -DisplayName "ConfigurationPort" -Profile Public -Direction Inbound -Action Allow -Protocol TCP -LocalPort 60081
Set-HostsEntry -IPAddress 127.0.0.1 -HostName 'configuration.kz.local' 

Install-IisAppPool -Name "web.kz"
Install-IisWebsite -Name "www.kz.local" -PhysicalPath "$basePath\Kz.Web" -AppPoolName "web.kz" -Bindings @('http/*:80:www.kz.local','http/*:60082:')
New-NetFirewallRule -DisplayName "WebPort" -Profile Public -Direction Inbound -Action Allow -Protocol TCP -LocalPort 60082
Set-HostsEntry -IPAddress 127.0.0.1 -HostName 'www.kz.local' 