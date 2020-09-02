using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180403140000_Integrations
{
    [Migration(2018, 04, 03, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventPartners")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("PartnerName").AsString(200)
                .WithColumn("AccessKey").AsString(int.MaxValue).Nullable()
                .WithColumn("SecretKey").AsString(int.MaxValue).Nullable()
                .WithColumn("Description").AsInt32().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventIntegrationDetails")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
                .WithColumn("PartnerId").AsInt32().ForeignKey("EventPartners", "Id")
                .WithColumn("AccessKey").AsString(int.MaxValue).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("AccessTokens")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("Token").AsString(int.MaxValue)
                .WithColumn("PartnerId").AsInt32().ForeignKey("EventPartners", "Id")
                .WithColumn("TokenType").AsString(100).Nullable()
                .WithColumn("ExpiresIn").AsString(100).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
