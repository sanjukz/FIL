using FIL.Contracts.Enums;
using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20181023163500_Transactions
{
    [Migration(2018, 10, 23, 16, 35, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Transactions").Column("ReportExportStatus").Exists())
            {
                Alter.Table("Transactions").AddColumn("ReportExportStatus").AsInt16().ForeignKey("ReportExportStatuses", "Id").WithDefaultValue(0);
            }
        }
    }
}
