using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrator_20200417140000_EventStripeAccountMappings
{
    [Migration(2020, 04, 17, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventStripeAccountMappings")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                  .WithColumn("Channeld").AsInt16().ForeignKey("Channels", "Id")
                  .WithColumn("SiteId").AsInt16().ForeignKey("Sites", "Id")
                  .WithColumn("StripeAccountId").AsInt16().ForeignKey("StripeAccounts", "Id")
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
