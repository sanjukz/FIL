using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20181231130000_PlaceWeekOff
{

    [Migration(2018, 12, 31, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("PlaceWeekOffs")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
               .WithColumn("WeekOffDay").AsInt16().ForeignKey("WeekOffDays", "Id").Nullable()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
