using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180914180000_ScanningAPIAddNewColumn
{
    [Migration(2018, 09, 14, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("MatchSeatTicketDetails").Column("EntryGateName").Exists())
            {
                Alter.Table("MatchSeatTicketDetails").AddColumn("EntryGateName").AsString(100).Nullable();
            }
            if (!Schema.Table("OfflineBarcodeDetails").Column("EntryGateName").Exists())
            {
                Alter.Table("OfflineBarcodeDetails").AddColumn("EntryGateName").AsString(100).Nullable();
            }
        }

    }
}
