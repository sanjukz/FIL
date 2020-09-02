$sentryAuthToken= (Get-SSMParameterValue -Name "/sentry/authToken").Parameters[0].Value
$sentryOrg= (Get-SSMParameterValue -Name "/sentry/organization").Parameters[0].Value

Set-Item -Path Env:SENTRY_AUTH_TOKEN -Value $sentryAuthToken
Set-Item -Path Env:SENTRY_ORG -Value $sentryOrg

# Version from git
$VERSION = (Get-ChildItem Env:REVISION).Value

# Sentry project name to be deployed
$PROJECT = (Get-ChildItem Env:SENTRY_PROJECT).Value

# create a new release version
sentry-cli releases new "$PROJECT@$VERSION" --project $PROJECT

# aggregate all the commits
sentry-cli releases set-commits --auto "$PROJECT@$VERSION" 

# Upload source maps
#sentry-cli releases files "$PROJECT@$VERSION" upload-sourcemaps ./dist --rewrite

# Finalize release
sentry-cli releases finalize "$PROJECT@$VERSION"
