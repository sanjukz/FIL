using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190520090000_SiteProperties
{
    [Migration(2019, 05, 20, 09, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("SiteProperties").Exists())
            {
                Create.Table("SiteProperties")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("SiteId").AsInt16().ForeignKey("Sites", "Id")
                .WithColumn("AltId").AsGuid()
                .WithColumn("Name").AsString(100)
                .WithColumn("Title").AsString(500)
                .WithColumn("Url").AsString(100)
                .WithColumn("Description").AsString(int.MaxValue).Nullable()
                .WithColumn("GoogleSiteVerification").AsString(100).Nullable()
                .WithColumn("HrefLang").AsString(100).Nullable()
                .WithColumn("Keyword").AsString(500).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    }
}