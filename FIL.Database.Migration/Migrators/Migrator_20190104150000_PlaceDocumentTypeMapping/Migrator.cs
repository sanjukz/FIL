using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190104150000_PlaceDocumentTypeMapping
{

    [Migration(2019, 01, 04, 15, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("PlaceDocumentTypeMappings")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
               .WithColumn("DocumentType").AsInt16().ForeignKey("DocumentTypes", "Id").Nullable()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
