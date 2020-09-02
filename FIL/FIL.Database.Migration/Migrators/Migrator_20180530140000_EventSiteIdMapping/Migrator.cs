using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180530140000_EventSiteIdMapping
{
    [Migration(2018, 05, 30, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventSiteIdMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("SortOrder").AsInt16()
                .WithColumn("SiteId").AsInt16().ForeignKey("Sites", "Id")                
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
