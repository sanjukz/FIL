using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190423110000_PlaceCalendarTiming
{
    [Migration(2019, 04, 23, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Days")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("AltId").AsGuid()
                  .WithColumn("Name").AsString(100).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("PlaceWeekOpenDays")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("AltId").AsGuid()
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("DayId").AsInt64().ForeignKey("Days", "Id")
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("DayTimeMappings")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("AltId").AsGuid()
                 .WithColumn("PlaceWeekOpenDayId").AsInt64().ForeignKey("PlaceWeekOpenDays", "Id")
                 .WithColumn("StartTime").AsString(100).Nullable()
                 .WithColumn("EndTime").AsString(100).Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("PlaceSeasonDetails")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("AltId").AsGuid()
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("Name").AsString(100).Nullable()
                 .WithColumn("StartDate").AsDateTime()
                 .WithColumn("EndDate").AsDateTime()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("PlaceSpecialDayTimeMappings")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("AltId").AsGuid()
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("Name").AsString(100).Nullable()
                 .WithColumn("SpecialDate").AsDateTime()
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
