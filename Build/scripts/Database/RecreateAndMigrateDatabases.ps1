param(
    [string] $serverName = 'localhost',
    [String] $config = 'Debug',
    [String] $basePath = "C:\Kyazoonga\Kz"
)

pushd $PSScriptRoot
./DeleteDatabase.ps1 $serverName 'KzOLTP' 
./DeleteDatabase.ps1 $serverName 'KzConfiguration'
./CreateDatabase.ps1 $serverName 'KzOLTP' 'IIS APPPOOL\\api.kz'
./CreateDatabase.ps1 $serverName 'KzConfiguration' 'IIS APPPOOL\\configuration.kz'

./BuildAndRunConfiguationMigrator.ps1 $config $basePath
./BuildAndRunOltpMigrator.ps1 $config $basePath
./MigrateConfiguration.ps1 '' #'DEVSQLCONNECTIONHERE'
popd