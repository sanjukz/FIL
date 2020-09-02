using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180905170000_TicketAlert
{
    [Migration(2018, 09, 05, 17, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("TicketAlertUserMappings").Column("NumberOfTickets").Exists())
            {
                Alter.Table("TicketAlertUserMappings")
                .AddColumn("NumberOfTickets").AsInt16().Nullable();
            }
        }
    }
}
