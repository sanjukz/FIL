using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180914163000_EventTicketAttributes
{
    [Migration(2018, 09, 14, 16, 30, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Alter.Table("EventTicketAttributes").AlterColumn("SharedInventoryGroupId").AsInt32().Nullable();
            Alter.Table("EventTicketAttributes").AlterColumn("AvailableTicketForSale").AsInt32();
            Alter.Table("EventTicketAttributes").AlterColumn("RemainingTicketForSale").AsInt32();
        }
    }
}
