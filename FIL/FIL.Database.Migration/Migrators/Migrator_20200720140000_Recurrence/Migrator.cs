using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20200720140000_Recurrence
{
    [Migration(2020, 07, 20, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventSchedules")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("EventFrequencyTypeId").AsInt16().ForeignKey("EventFrequencyTypes", "Id")
                 .WithColumn("OccuranceTypeId").AsInt16().ForeignKey("OccuranceTypes", "Id")
                 .WithColumn("Name").AsString(int.MaxValue).Nullable()
                 .WithColumn("StartDateTime").AsDateTime().Nullable()
                 .WithColumn("EndDateTime").AsDateTime().Nullable()
                 .WithColumn("DayId").AsString(100).Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ScheduleDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("EventScheduleId").AsInt64().ForeignKey("EventSchedules", "Id")
                .WithColumn("StartDateTime").AsDateTime()
                .WithColumn("EndDateTime").AsDateTime()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}