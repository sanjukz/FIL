pushd $PSScriptRoot
pushd "../../"

ForEach ($folder in (Get-ChildItem -Path "*.Test*" -Directory)) { 
    
    dotnet restore $folder.FullName 
    
    if ($lastexitcode -eq 1) {
        Write-Host "dotnet restore command Failed for $folder"
        exit 1
    }
    
    dotnet build $folder.FullName 
    
    if ($lastexitcode -eq 1) {
        Write-Host "dotnet build command Failed for $folder"
        exit 1
    }

    dotnet test $folder.FullName  
   
    if ($lastexitcode -eq 1) {
        Write-Host "dotnet test command Failed for $folder"
        exit 1
    }
}

popd