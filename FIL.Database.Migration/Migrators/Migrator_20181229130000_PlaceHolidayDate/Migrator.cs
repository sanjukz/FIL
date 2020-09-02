using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20181229130000_PlaceHolidayDate
{

    [Migration(2018, 12, 29, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("PlaceHolidayDates")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("EventDetailId").AsInt64()
               .WithColumn("LeaveDateTime").AsDateTime()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
