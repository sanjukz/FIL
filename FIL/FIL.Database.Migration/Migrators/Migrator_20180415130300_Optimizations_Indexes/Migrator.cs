using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180415130300_Optimizations_Indexes
{
    [Migration(2018, 04, 15, 13, 3, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Index()
                .OnTable("FeaturedEvents")
                .OnColumn("SiteId");

            Create.Index()
                .OnTable("EventDetails")
                .OnColumn("EventId");

            Create.Index()
                .OnTable("EventTicketDetails")
                .OnColumn("EventDetailId");

            Create.Index()
                .OnTable("EventTicketAttributes")
                .OnColumn("EventTicketDetailId");
        }
    }
}
