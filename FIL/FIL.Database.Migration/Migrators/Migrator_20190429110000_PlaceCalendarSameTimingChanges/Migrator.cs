using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190429110000_PlaceCalendarSameTimingChanges
{
    [Migration(2019, 04, 29, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("PlaceWeekOpenDays").Column("IsSameTime").Exists())
            {
                Alter.Table("PlaceWeekOpenDays").AddColumn("IsSameTime").AsBoolean().Nullable();
            }
            if (!Schema.Table("PlaceSeasonDetails").Column("IsSameTime").Exists())
            {
                Alter.Table("PlaceSeasonDetails").AddColumn("IsSameTime").AsBoolean().Nullable();
            }
        }
    }
}
