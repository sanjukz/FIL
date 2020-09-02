# Run PowerShell as administrator
# From command line, run "Set-ExecutionPolicy Unrestricted"

param (
    [String] $basePath = "C:\Kyazoonga\Kz"
)

$Policy = "Unrestricted"
If ((get-ExecutionPolicy) -ne $Policy) {
  Write-Host "Script Execution is disabled. Enabling it now"
  Set-ExecutionPolicy $Policy -Force
  Write-Host "Please Re-Run this script in a new powershell enviroment"
  Exit
}

## Setup
# Make sure temp exists
New-Item c:\temp -type directory

## INSTALLATIONS
# Make sure Chocolatey package manager is installed
Write-Host "Installing Chocolatey"
iex "$PSScriptRoot/scripts/InstallChocolatey.ps1"
Write-Host

# Make sure Redis is installed
Write-Host "Installing Redis"
iex "$PSScriptRoot/scripts/InstallRedis.ps1"
Write-Host

Write-Host "Installing applications from Chocolatey"
# Install Git
cinst git -y

# Carbon powershell modules
cinst Carbon -y

# .NET Core Runtimes
cinst dotnetcore-runtime -y
# 1.0.1 SDK
cinst dotnetcoresdk -y
# 2.0.0 SDK
cinst dotnetcore-sdk -y
cinst nuget.commandline -y
cinst dotnetcore-windowshosting -y

# Javascript Things
cinst nodejs -y
cinst yarn -y
Write-Host

do {
    $createSiteData = Read-Host "Do you want to install SQLExpress? (Y/N)"
} while ($createSiteData -ne "Y" -and $createSiteData -ne "N")
if ($createSiteData -eq "Y") {
    cinst SqlServer2016Express
}
Write-Host

do {
    $createSiteData = Read-Host "Do you want to install SQL Management Studio? (Y/N)"
} while ($createSiteData -ne "Y" -and $createSiteData -ne "N")
if ($createSiteData -eq "Y") {
    cinst sql-server-management-studio
}
Write-Host

do {
    $createSiteData = Read-Host "Do you want to install Visual Studio Code? (Y/N)"
} while ($createSiteData -ne "Y" -and $createSiteData -ne "N")
if ($createSiteData -eq "Y") {
    cinst visualstudiocode
}

# Install IIS Features
Write-Host "Installing IIS"
iex "$PSScriptRoot/scripts/InstallIIS.ps1"
Write-Host

Write-Host "Installing MSBuild helpers. Please enter A to accept all once prompted."
# Needed to build from command line
Install-Module -Name Invoke-MsBuild

Import-Module 'Carbon'

## Firewall rules
New-NetFirewallRule -Name "SQL Remote Connection" -Profile Public -Direction Inbound -Action Allow -Protocol TCP -LocalPort 1433
New-NetFirewallRule -Name "Redis" -Profile Public -Direction Inbound -Action Allow -Protocol TCP -LocalPort 6379

## Windows authentication


## IIS Sites
iex "$PSScriptRoot/scripts/LocalSetupSites.ps1 $basePath"
## TODO: Static file directory bindings

## SSL certs?
## TODO: Need self signed for local

## Check Databases
iex "$PSScriptRoot/scripts/Database/CreateDatabase.ps1 'localhost' 'KzOLTP' 'IIS APPPOOL\\api.kz'"
iex "$PSScriptRoot/scripts/Database/CreateDatabase.ps1 'localhost' 'KzConfiguration' 'IIS APPPOOL\\configuration.kz'"

Import-Module 'Invoke-MsBuild'

Write-Host "Time to build!"
$buildResult = Invoke-MsBuild -Path "$basePath\Kz.sln" -Params "/nologo /verbosity:Quiet /clp:ErrorsOnly"

if ($buildResult.BuildSucceeded -eq $true)
{
    Write-Output ("Build completed successfully in {0:N1} seconds." -f $buildResult.BuildDuration.TotalSeconds)
}
elseif ($buildResult.BuildSucceeded -eq $false)
{
    Write-Output ("Build failed after {0:N1} seconds. Check the build log file '$($buildResult.BuildLogFilePath)' for errors." -f $buildResult.BuildDuration.TotalSeconds)
}

iex "$PSScriptRoot/scripts/Database/RecreateAndMigrateDatabases.ps1 'localhost' 'Debug' $basePath"

iisreset >NUL