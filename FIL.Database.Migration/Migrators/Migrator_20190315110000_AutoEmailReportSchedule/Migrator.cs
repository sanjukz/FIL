using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190315110000_AutoEmailReportSchedule
{
    [Migration(2019, 03, 15, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("AutoEmailReportEventDetailMapping").Exists())
            {
                Create.Table("AutoEmailReportEventDetailMapping")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()                    
                    .WithColumn("AutoEmailReportScheduleId").AsInt32().ForeignKey("AutoEmailReportSchedule", "Id")
                    .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")                   
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    }
}
