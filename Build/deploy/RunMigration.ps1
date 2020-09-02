param (
    [string] $branchName = $null,
    [string] $region = "us-west-2"
)

pushd $PSScriptRoot

$conn = (aws ssm get-parameters --region $region --names "/$branchName/connectionString" --with-decryption | ConvertFrom-Json).Parameters[0].Value

pushd "../../Kz.Database.Migration"

dotnet run -- -c $conn

if ($lastexitcode -eq 1) {
    Write-Host "Kz Database Migration Failed"
    exit 1
}

popd

pushd "../../Kz.Configuration.Database.Migration"

$conn = (aws ssm get-parameters --region $region --names "/$branchName/configurationConnectionString" --with-decryption | ConvertFrom-Json).Parameters[0].Value

dotnet run -- -c $conn

if ($lastexitcode -eq 1) {
    Write-Host "Kz Configuration Database Migration Failed"
    exit 1
}

popd
