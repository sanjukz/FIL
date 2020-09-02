using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190314130000_AutoEmailReportSchedule
{
    [Migration(2019, 03, 14, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("AutoEmailReportSchedule").Exists())
            {
                Create.Table("AutoEmailReportSchedule")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                    .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                    .WithColumn("Name").AsString(100)
                    .WithColumn("SubjectLine").AsString(100)
                    .WithColumn("UserEmailIds").AsString(int.MaxValue)
                    .WithColumn("UserCCEmailIds").AsString(int.MaxValue)
                    .WithColumn("UserBCCEmailIds").AsString(int.MaxValue)
                    .WithColumn("ReportFrequencyTypeId").AsInt16().ForeignKey("ReportFrequencyTypes", "Id")
                    .WithColumn("ReportOccurrenceIntervalId").AsInt16().ForeignKey("ReportOccurrenceIntervals", "Id")
                    .WithColumn("OccurrenceValue").AsString(100)
                    .WithColumn("OccurrenceStartTime").AsDateTime()
                    .WithColumn("OccurrenceEndtime").AsDateTime()
                    .WithColumn("StartDate").AsDateTime()
                    .WithColumn("EndDate").AsDateTime()
                    .WithColumn("SentOutTimeDifference").AsInt32()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    }
}
