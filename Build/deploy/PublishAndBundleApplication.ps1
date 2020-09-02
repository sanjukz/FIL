param (
    [string] $applicationName = $null,
    [string] $branchName = $null,
    [string] $region = $null
)

pushd $PSScriptRoot
pushd "../../$applicationName"

dotnet publish -o site-deploy -c Release

$domain = (Get-SSMParameterValue -Name "/$branchName/domain").Parameters[0].Value
$user = (Get-SSMParameterValue -Name "/$branchName/domainjoinuser").Parameters[0].Value
$pwd= (Get-SSMParameterValue -Name "/$branchName/domainjoinpwd").Parameters[0].Value
(Get-Content "./site-deploy/.ebextensions/01-JoinDomain.config").replace('$DOMAIN', $domain).replace('$USER', $user).replace('$PWD', $pwd) | Set-Content "./site-deploy/.ebextensions/01-JoinDomain.config"
(Get-Content "./site-deploy/.ebextensions/07-AddSystemVariable.config").replace('$NEW_RELIC_APP_NAME', $applicationName) | Set-Content "./site-deploy/.ebextensions/07-AddSystemVariable.config"
if ($lastexitcode -eq 1) {
    Write-Host "dotnet publish command Failed for $applicationName"
    exit 1
}

pushd ./site-deploy

$awsAccessKey = (aws ssm get-parameters --region $region --names "/$branchName/awsAccessKey" --with-decryption | ConvertFrom-Json).Parameters[0].Value
$awsAccessSecret = (aws ssm get-parameters --region $region --names "/$branchName/awsAccessSecret" --with-decryption | ConvertFrom-Json).Parameters[0].Value

$optionsSettings = @(
    @{
        "option_name"="AWS_ACCOUNT_SECRET";
        "value"=$awsAccessSecret;
    }, @{
        "option_name"="AWS_ACCOUNT_ID";
        "value"=$awsAccessKey;
    })

If ($applicationName -like 'Kz.Web*') {
    # TODO: Load balancer configs.  *.kz.com for all but Kz.Web (ie. itms.kz.com, retail.kz.com, etc)
    # $sslKeyArn = (aws ssm get-parameters --region us-east-1 --names "/$branchName/sslKeyArn" | ConvertFrom-Json).Parameters[0].Value
    # $enableHttp = (aws ssm get-parameters --region us-east-1 --names "/$branchName/enableHttp" | ConvertFrom-Json).Parameters[0].Value
    # (Get-Content "./.ebextensions/03-LoadBalancer.config")
    #    .replace('$SSL_CERT_ARN', $sslKeyArn)
    #    .replace('$HTTP_ENABLED', $enableHttp) | Set-Content "./.ebextensions/03-LoadBalancer.config"
    # LB file contents:
    #
    #option_settings:
    #    aws:elb:listener:443:
    #        ListenerProtocol: HTTPS
    #        SSLCertificateId: $SSL_CERT_ARN
    #        InstancePort: 80
    #        InstanceProtocol: HTTP
    #    aws:elb:listener:80:
    #        ListenerEnabled: $HTTP_ENABLED

    $apiBaseUrl = (aws ssm get-parameters --region $region --names "/$branchName/apiBaseUrl" | ConvertFrom-Json).Parameters[0].Value
    $awsKeyKmsName = (aws ssm get-parameters --region $region --names "/$branchName/awsKeyKmsName" | ConvertFrom-Json).Parameters[0].Value
    $awsKeyBucketName = (aws ssm get-parameters --region $region --names "/$branchName/awsKeyBucketName" | ConvertFrom-Json).Parameters[0].Value
    $awsKeyKmsId = (aws ssm get-parameters --region $region --names "/$branchName/awsKeyKmsId" | ConvertFrom-Json).Parameters[0].Value
    $sentryPublic = (aws ssm get-parameters --region $region --names "/$branchName/sentry/publicDsn" | ConvertFrom-Json).Parameters[0].Value
    $awsStaticBucketName = (aws ssm get-parameters --region $region --names "/$branchName/awsStaticBucketName" | ConvertFrom-Json).Parameters[0].Value
    $googleTagManagerId = (aws ssm get-parameters --region $region --names "/$branchName/gtmId" | ConvertFrom-Json).Parameters[0].Value

    $optionsSettings += @{
        "option_name"="API_ENDPOINT";
        "value"=$apiBaseUrl;
    }
    $optionsSettings += @{
        "option_name"="SENTRY_PUBLIC_DSN";
        "value"=$sentryPublic;
    }
    $optionsSettings += @{
        "option_name"="AWS_KEY_BUCKET_NAME";
        "value"=$awsKeyBucketName;
    }
    $optionsSettings += @{
        "option_name"="AWS_STATIC_BUCKET_NAME";
        "value"=$awsStaticBucketName;
    }
    $optionsSettings += @{
        "option_name"="AWS_KMS_KEY_NAME";
        "value"=$awsKeyKmsName;
    }
    $optionsSettings += @{
        "option_name"="AWS_KMS_KEY_ID";
        "value"=$awsKeyKmsId;
    }
    # Feel specific 
    If ($applicationName -like '*Feel*') {
        $googleTagManagerId = (aws ssm get-parameters --region $region --names "/$branchName/feel/gtmId" | ConvertFrom-Json).Parameters[0].Value

        $optionsSettings += @{
            "option_name"="GTM_ACCOUNT_ID";
            "value"=$googleTagManagerId;
        }

        $baseStaticUrl = (aws ssm get-parameters --region $region --names "/$branchName/feel/baseStaticUrl" | ConvertFrom-Json).Parameters[0].Value          
        $optionsSettings += @{
            "option_name"="BASE_STATIC_URL";
            "value"=$baseStaticUrl;
        }

        $stripePublicToken = (aws ssm get-parameters --region $region --names "/$branchName/feel/stripePublicToken" | ConvertFrom-Json).Parameters[0].Value
        $optionsSettings += @{
            "option_name"="STRIPE_PUBLIC_TOKEN";
            "value"=$stripePublicToken;
        }

        $iaSearchUrl = (aws ssm get-parameters --region $region --names "/$branchName/feel/iaSearchUrl" | ConvertFrom-Json).Parameters[0].Value          
        $optionsSettings += @{
            "option_name"="SEARCH_ENDPOINT";
            "value"=$iaSearchUrl;
        }
    } Else {
        $googleTagManagerId = (aws ssm get-parameters --region $region --names "/$branchName/gtmId" | ConvertFrom-Json).Parameters[0].Value
        $optionsSettings += @{
            "option_name"="GTM_ACCOUNT_ID";
            "value"=$googleTagManagerId;
        }

        # Kz.Web only
        If ($applicationName -eq 'Kz.Web') {
            $baseStaticUrl = (aws ssm get-parameters --region $region --names "/$branchName/baseStaticUrl" | ConvertFrom-Json).Parameters[0].Value
            $optionsSettings += @{
                "option_name"="BASE_STATIC_URL";
                "value"=$baseStaticUrl;
            }
            
            $stripePublicToken = (aws ssm get-parameters --region $region --names "/$branchName/stripePublicToken" | ConvertFrom-Json).Parameters[0].Value
            $optionsSettings += @{
                "option_name"="STRIPE_PUBLIC_TOKEN";
                "value"=$stripePublicToken;
            }
        }
    }
}

# Normal API

If ($applicationName -eq 'Kz.Api') {
#   Currently this comes from the Configuration API :)
#    $connectionString = (aws ssm get-parameters --region $region --names "/$branchName/connectionString" --with-decryption | ConvertFrom-Json).Parameters[0].Value
#
#    $optionsSettings += @{
#        "option_name"="CONNECTION_STRING";
#        "value"=$connectionString;
#    }
}

# Configuration API
If ($applicationName -eq 'Kz.Configuration.Api') {
    $connectionString = (aws ssm get-parameters --region $region --names "/$branchName/configurationConnectionString" --with-decryption | ConvertFrom-Json).Parameters[0].Value
    
    $optionsSettings += @{
        "option_name"="CONFIG_CONNECTION_STRING";
        "value"=$connectionString;
    }
}

$sentryPrivate = (aws ssm get-parameters --region $region --names "/$branchName/sentry/privateDsn" | ConvertFrom-Json).Parameters[0].Value
$configBaseUrl = (aws ssm get-parameters --region $region --names "/$branchName/configurationBaseUrl" | ConvertFrom-Json).Parameters[0].Value

$optionsSettings += @{
    "option_name"="KZ_CONFIGURATION_SETTING_EXPIRATION_MINUTES";
    "value"=30;
}

$optionsSettings += @{
    "option_name"="HOSTING_ENVIRONMENT";
    "value"="AWS";
}

$optionsSettings += @{
    "option_name"="CONFIGURATION_API_ENDPOINT";
    "value"=$configBaseUrl;
}

$optionsSettings += @{
    "option_name"="SENTRY_DSN";
    "value"=$sentryPrivate;
}

$aspNetCoreEnvironment = "Development"
If ($branchName -like 'master*') {
    $aspNetCoreEnvironment = "Production"
}

$optionsSettings += @{
    "option_name"="ASPNETCORE_ENVIRONMENT";
    "value"=$aspNetCoreEnvironment;
}

$environmentVars = @{
    "option_settings" = $optionsSettings
}

# https://github.com/cloudbase/powershell-yaml
# Set-PSRepository -Name PSGallery -InstallationPolicy Trusted
# Install-Module powershell-yaml
Import-Module powershell-yaml

if ($lastexitcode -eq 1) {
    Write-Host "Import powershell-yaml Failed"
    exit 1
}

ConvertTo-Yaml $environmentVars | Out-File -FilePath "./.ebextensions/99-EnvironmentVariables.config"

if ($lastexitcode -eq 1) {
    Write-Host "ConvertTo-Yaml Failed"
    exit 1
}
popd

popd
