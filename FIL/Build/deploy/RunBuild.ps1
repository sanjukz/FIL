pushd $PSScriptRoot
pushd "../../"

dotnet restore

if ($lastexitcode -eq 1) {
    Write-Host "dotnet restore command Failed"
    exit 1
}

dotnet build -c Release # always build in release to build npm

if ($lastexitcode -eq 1) {
    Write-Host "dotnet build command Failed"
    exit 1
}

popd