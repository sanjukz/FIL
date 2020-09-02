using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190109150000_PlaceCustomerDocumentTypeMappings
{

    [Migration(2019, 01, 09, 15, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("PlaceCustomerDocumentTypeMappings")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
               .WithColumn("CustomerDocumentType").AsInt64().ForeignKey("CustomerDocumentTypes", "Id").Nullable()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
