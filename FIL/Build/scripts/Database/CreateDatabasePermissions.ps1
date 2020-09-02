param(
    [string] $serverName = $null,
    [string] $dbName = $null,
    [string] $user = $null,
    [string] $role = "db_execproc"
)

# Create DB login
Write-Host "Creating $role permissions for $user..."
$connectionString = "Server=$serverName;Database=$dbName;Integrated Security=True;Trusted_Connection=True;"
$connection = New-Object System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$connection.Open()

$query = @"
IF NOT EXISTS (SELECT * FROM [master].sys.server_principals WHERE name = '$user')
BEGIN
    CREATE LOGIN [$user] FROM WINDOWS WITH DEFAULT_DATABASE=[$dbName], DEFAULT_LANGUAGE=[us_english]
END
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '$user')
BEGIN
    CREATE USER [$user] FOR LOGIN [$user] WITH DEFAULT_SCHEMA=[dbo]
END
ALTER ROLE [$role] ADD MEMBER [$user]
"@

$command = $connection.CreateCommand()
$command.CommandText = $query
$result = $command.ExecuteReader()
$connection.Close()