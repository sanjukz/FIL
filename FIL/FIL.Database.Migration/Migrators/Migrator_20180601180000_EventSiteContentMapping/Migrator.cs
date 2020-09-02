using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180601180000_EventSiteContentMapping
{
    [Migration(2018, 06, 01, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventSiteContentMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("SiteId").AsInt16().ForeignKey("Sites", "Id")
                .WithColumn("SiteTitle").AsString(100)
                .WithColumn("SiteLogo").AsString(100)
                .WithColumn("BannerText").AsString(100)
                .WithColumn("SiteLevel").AsInt16().ForeignKey("SiteLevels", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
