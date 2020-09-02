using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190526123000_Indexing
{
    [Migration(2019, 05, 26, 12, 30, 00)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Index()
               .OnTable("DynamicStadiumCoordinates")
               .OnColumn("VenueId");
        }
    }
}
