using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180731130000_Transaction
{
    [Migration(2018, 07, 31, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {            
            if (!Schema.Table("Transactions").Column("AltId").Exists())
            {
                Alter.Table("Transactions")
                .AddColumn("AltId")
                .AsGuid()
                .NotNullable()
                .WithDefaultValue(FluentMigrator.SystemMethods.NewGuid);
                //.SetExistingRowsTo(FluentMigrator.SystemMethods.NewGuid);
            }
        }
    }
}
