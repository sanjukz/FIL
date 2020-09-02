param(
    [string] $serverName = $null,
    [string] $dbName = $null,
    [string] $user = $null
)
 
# Create empty database
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
$srv = New-Object Microsoft.SqlServer.Management.Smo.Server($serverName)
if ($srv.Databases["$dbName"]) {
    Write-Host "$dbName already exists on $serverName"
    exit 0
}

$db = New-Object Microsoft.SqlServer.Management.Smo.Database($srv, $dbName)
$db.Create()
Write-Host "Created [$dbName] on $serverName..."

# Create role for executing sprocs
Write-Host "Creating db_execproc role for [$dbName]..."
$connectionString = "Server=$serverName;Database=$dbName;Integrated Security=True;Trusted_Connection=True;"
$connection = New-Object System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$connection.Open()

$query = @"
CREATE ROLE [db_execproc] AUTHORIZATION [dbo]
GRANT EXECUTE ON SCHEMA::dbo TO db_execproc;
"@

$command = $connection.CreateCommand()
$command.CommandText = $query
$result = $command.ExecuteReader()
$connection.Close()

pushd $PSScriptRoot
.\CreateDatabasePermissions.ps1 $serverName $dbName $user "db_execproc"
.\CreateDatabasePermissions.ps1 $serverName $dbName $user "db_datareader"
.\CreateDatabasePermissions.ps1 $serverName $dbName $user "db_datawriter"
popd