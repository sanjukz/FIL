using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190316120000_CustomerInformation
{
    [Migration(2019, 03, 16, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("CustomerInformations")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("Name").AsString(int.MaxValue).Nullable()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventCustomerInformationMappings")
              .WithColumn("Id").AsInt64().PrimaryKey().Identity()
              .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
              .WithColumn("CustomerInformationId").AsInt64().ForeignKey("CustomerInformations", "Id")
              .WithColumn("IsEnabled").AsBoolean().Indexed()
              .WithColumn("CreatedUtc").AsDateTime()
              .WithColumn("UpdatedUtc").AsDateTime().Nullable()
              .WithColumn("CreatedBy").AsGuid()
              .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
