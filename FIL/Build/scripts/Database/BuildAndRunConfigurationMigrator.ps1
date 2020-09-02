param(
    [string] $config = 'Debug',
    [String] $basePath = "C:\Kyazoonga\Kz"
)

Import-Module 'Invoke-MsBuild'
$buildResult = Invoke-MsBuild -Path "$basePath\Kz.Configuration.Database.Migration\Kz.Database.Migration.csproj" -Params "/nologo /verbosity:Quiet /clp:ErrorsOnly"

if ($buildResult.BuildSucceeded -eq $true)
{
    Write-Output ("Build completed successfully in {0:N1} seconds. Running Configuration Migrator." -f $buildResult.BuildDuration.TotalSeconds)

    pushd "$basePath\Kz.Configuration.Database.Migration\bin\$config"
    iex "./Kz.Configuration.Database.Migration.exe"
    popd
}
elseif ($buildResult.BuildSucceeded -eq $false)
{
    Write-Output ("Build failed after {0:N1} seconds. Check the build log file '$($buildResult.BuildLogFilePath)' for errors." -f $buildResult.BuildDuration.TotalSeconds)
}

