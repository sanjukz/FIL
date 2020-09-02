using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.MIgrator_20180809180000_Redemption
{
    [Migration(2018, 08, 09, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("MatchSeatTicketDetails").Column("ConsumedDateTime").Exists())
            {
                Alter.Table("MatchSeatTicketDetails").AddColumn("ConsumedDateTime").AsDateTime().Nullable();
            }
            if (!Schema.Table("OfflineBarcodeDetails").Column("ConsumedDateTime").Exists())
            {
                Alter.Table("OfflineBarcodeDetails").AddColumn("ConsumedDateTime").AsDateTime().Nullable();
            }
        }

    }
}
