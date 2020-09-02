using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190315120000_PlaceHolidayAndPlaceWeekOffChanges
{
    [Migration(2019, 03, 15, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {

            if (!Schema.Table("PlaceWeekOffs").Column("EventId").Exists())
            {
                Alter.Table("PlaceWeekOffs").AddColumn("EventId").AsInt64().ForeignKey("Events", "Id").Nullable();
            }

            if (!Schema.Table("PlaceHolidayDates").Column("EventId").Exists())
            {
                Alter.Table("PlaceHolidayDates").AddColumn("EventId").AsInt64().ForeignKey("Events", "Id").Nullable();
            }
        }
    }
}
