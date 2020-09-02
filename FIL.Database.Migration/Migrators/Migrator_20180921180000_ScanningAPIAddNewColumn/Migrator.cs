using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180921180000_ScanningAPIAddNewColumn
{
    [Migration(2018, 09, 21, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("OfflineBarcodeDetails").Column("TicketTypeId").Exists())
            {
                Alter.Table("OfflineBarcodeDetails").AddColumn("TicketTypeId").AsInt16().Nullable();
            }
        }
    }
}


