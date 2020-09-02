using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrator_20200512160000_LiveEventDetails
{
    [Migration(2020, 05, 12, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("LiveEventDetails")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                  .WithColumn("OnlineEventTypeId").AsInt16().ForeignKey("OnlineEventTypes", "Id")
                  .WithColumn("VideoId").AsString(200).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
