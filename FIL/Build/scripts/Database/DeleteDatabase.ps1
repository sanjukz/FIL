param(
    [string] $serverName = $null,
    [string] $dbName = $null
)
 
# Create empty database
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
$srv = New-Object Microsoft.SqlServer.Management.Smo.Server($serverName)

try {
    $db = $srv.databases["$dbName"]
    if(-Not ($db -eq $null)) {
        $srv.KillAllProcesses("$dbName")
        $db.Drop()
        Write-Host "Dropped database $dbName from $serverName"
    } else {
        Write-Host "Database $dbName not found on $serverName"
    }
}
catch {
    Write-Host "ERROR: Deletion of $databaseName failed"
    throw
}
