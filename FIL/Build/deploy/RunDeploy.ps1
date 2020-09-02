param (
    [string] $branchName = $null,
    [string] $region = "us-west-2"
)

pushd $PSScriptRoot

# https://github.com/cloudbase/powershell-yaml
Set-PSRepository -Name PSGallery -InstallationPolicy Trusted

if (Get-Module -ListAvailable -Name powershell-yaml) {
    Write-Host "Module exists"
} 
else {
    Write-Host "Module does not exist"
    Install-Module powershell-yaml
}

# Setting up env variables for sentry release
$VERSION = git rev-parse --short HEAD
[System.Environment]::SetEnvironmentVariable("REVISION", $VERSION, "machine")

$PROJECT= (Get-SSMParameterValue -Name "/$branchName/sentry/projectName").Parameters[0].Value
[System.Environment]::SetEnvironmentVariable("SENTRY_PROJECT", $PROJECT, "machine")

$sentryAuthToken= (Get-SSMParameterValue -Name "/sentry/authToken").Parameters[0].Value
Set-Item -Path Env:SENTRY_AUTH_TOKEN -Value $sentryAuthToken

$sentryOrg= (Get-SSMParameterValue -Name "/sentry/organization").Parameters[0].Value
Set-Item -Path Env:SENTRY_ORG -Value $sentryOrg

# ../scripts/InstallChocolatey.ps1
# cinst -y awscli # Make sure AWS CLI is installed
get-childitem -path "../../" -filter *site-deploy* | remove-item -force -recurse

./PublishAndBundleApplication.ps1 "Kz.Configuration.Api" $branchName $region

./PublishAndBundleApplication.ps1 "Kz.Api" $branchName $region

#./PublishAndBundleApplication.ps1 "Kz.Web" $branchName $region

./PublishAndBundleApplication.ps1 "Kz.Web.Kitms" $branchName $region

./PublishAndBundleApplication.ps1 "Kz.Web.Feel" $branchName $region

./PublishAndBundleApplication.ps1 "Kz.Web.Kitms.Feel" $branchName $region

if ($lastexitcode -eq 1) {
    Write-Host "dotnet publish command Failed for $applicationName"
    exit 1
} 
# Sentry Release
# create a new release version
sentry-cli releases new "$PROJECT@$VERSION" --project $PROJECT

# aggregate all the commits
sentry-cli releases set-commits --auto "$PROJECT@$VERSION" 

# Upload source maps
#sentry-cli releases files "$PROJECT@$VERSION" upload-sourcemaps ./dist --rewrite

# Finalize release
sentry-cli releases finalize "$PROJECT@$VERSION"
