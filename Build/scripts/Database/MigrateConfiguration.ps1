param(
    [string] $sourceConnectionString = $null,
    [string] $targetConnectionString = "Server=localhost;Database=KzConfiguration;Integrated Security=True;Trusted_Connection=True;" 
)

function MigrateConfigurationDB {
    param(
        [string] $sourceConnectionString = $null,
        [string] $targetConnectionString = $null,
        [string] $tableName = $null
    )
          
    try 
    {
        $sourceConnection = new-object system.data.SqlClient.SQLConnection($sourceConnectionString)
        $targetConnection = new-object system.data.SqlClient.SQLConnection($targetConnectionString)

        $sourceConnection.Open()
        if ($tableName -like "Configurations") {
            $getNonMigratableCommand = new-object system.data.sqlclient.sqlcommand("SELECT Id FROM ConfigurationSets WHERE CanMigrate=1", $sourceConnection)
            $ids = @()
            $tempReader = $getNonMigratableCommand.ExecuteReader()
            while ($tempReader.Read()) {
              $ids += @($tempReader["Id"].ToString())
            }
            $migrateableConfigSetIds = $ids -join ","
            $tempReader.Close()
            $sourceCommand = new-object system.data.sqlclient.sqlcommand("SELECT * FROM $tableName WHERE ConfigurationSetId IN ($migrateableConfigSetIds)", $sourceConnection)
        } else {
            $sourceCommand = new-object system.data.sqlclient.sqlcommand("SELECT * FROM $tableName",$sourceConnection)
        }
        
        #Fill the source table      
        $reader = $sourceCommand.ExecuteReader() 
        
        #Open target connection, pass the source table as parameter to stored procedure on target machine.
        $targetConnection.Open()

        #truncate table
        if ($tableName -like "ConfigurationSets") {
            $query = "DELETE FROM Configurations;DELETE FROM ConfigurationKeys;DELETE FROM $tableName;"
        } else {
            $query = "DELETE FROM $tableName;"
        }
        $command = new-object system.data.sqlclient.sqlcommand($query, $targetConnection)
        $command.ExecuteNonQuery()

        $bc = New-Object System.Data.SqlClient.SqlBulkCopy $targetConnection, $([System.Data.SqlClient.SqlBulkCopyOptions]::KeepIdentity -bor 0), $null
        $bc.DestinationTableName = $tableName
        $bc.WriteToServer($reader)
        
        $bc.Close()
        $reader.Close()
        $sourceConnection.Close()
        $targetConnection.Close()
    }
    catch 
    {
        Write-Error "Failed to migrate table $tableName"
        Write-Error $_.Exception.Message
        exit 1
    }
}

Write-Host "Migrating ConfigurationSets Table"
MigrateConfigurationDB $sourceConnectionString $targetConnectionString "ConfigurationSets"

Write-Host "Migrating ConfigurationKeys Table"
MigrateConfigurationDB $sourceConnectionString $targetConnectionString "ConfigurationKeys"

Write-Host "Migrating Configurations Table"
MigrateConfigurationDB $sourceConnectionString $targetConnectionString "Configurations"