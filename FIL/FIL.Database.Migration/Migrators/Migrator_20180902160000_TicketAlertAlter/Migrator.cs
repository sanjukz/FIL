using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180902160000_TicketAlert
{
    [Migration(2018, 09, 02, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("TicketAlertUserMappings").Column("IsInterestedTourAndTravel").Exists())
            {
                Alter.Table("TicketAlertUserMappings")
                .AddColumn("TourTravelPackage").AsInt16().ForeignKey("TourTravelPackages", "Id").Nullable();
            }
        }
    }
}
