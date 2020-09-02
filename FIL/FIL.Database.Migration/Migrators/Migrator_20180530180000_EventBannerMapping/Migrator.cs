using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180530180000_EventBannerMapping
{
    [Migration(2018, 05, 30, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventBannerMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("SiteId").AsInt16().ForeignKey("Sites", "Id")
                .WithColumn("BannerName").AsString(100)
                .WithColumn("SortOrder").AsInt16()                             
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
