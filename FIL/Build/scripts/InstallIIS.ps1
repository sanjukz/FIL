# --------------------------------------------------------------------
# Define the variables.
# --------------------------------------------------------------------
$InetPubRoot = "D:\Inetpub"
$InetPubLog = "D:\Inetpub\Log"
$InetPubWWWRoot = "D:\Inetpub\WWWRoot"

# --------------------------------------------------------------------
# Loading Feature Installation Modules
# --------------------------------------------------------------------
Import-Module ServerManager 

# --------------------------------------------------------------------
# Installing IIS
# --------------------------------------------------------------------
Add-WindowsFeature -Name Web-Common-Http,Web-Asp-Net,Web-Net-Ext,Web-ISAPI-Ext,Web-ISAPI-Filter,Web-Http-Logging,Web-Request-Monitor,Web-Basic-Auth,Web-Windows-Auth,Web-Filtering,Web-Performance,Web-Mgmt-Console,Web-Mgmt-Compat,RSAT-Web-Server,WAS,IIS-ASPNET45,IIS-WindowsAuthentication -IncludeAllSubFeature

# --------------------------------------------------------------------
# Loading IIS Modules
# --------------------------------------------------------------------
Import-Module WebAdministration

# --------------------------------------------------------------------
# Creating IIS Folder Structure
# --------------------------------------------------------------------
New-Item -Path $InetPubRoot -type directory -Force -ErrorAction SilentlyContinue
New-Item -Path $InetPubLog -type directory -Force -ErrorAction SilentlyContinue
New-Item -Path $InetPubWWWRoot -type directory -Force -ErrorAction SilentlyContinue