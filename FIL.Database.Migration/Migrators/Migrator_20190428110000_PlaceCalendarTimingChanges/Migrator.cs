using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190428110000_PlaceCalendarTiming
{
    [Migration(2019, 04, 28, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("SeasonDaysMappings")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("AltId").AsGuid()
                  .WithColumn("PlaceSeasonDetailId").AsInt64().ForeignKey("PlaceSeasonDetails", "Id")
                  .WithColumn("DayId").AsInt64().ForeignKey("Days", "Id")
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("SeasonDaysTImeMappings")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("AltId").AsGuid()
                 .WithColumn("SeasonDaysMappingId").AsInt64().ForeignKey("SeasonDaysMappings", "Id")
                 .WithColumn("StartTime").AsString(100).Nullable()
                 .WithColumn("EndTime").AsString(100).Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
