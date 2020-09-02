using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190509110000_Tags
{
    [Migration(2019, 05, 09, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Tags")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("AltId").AsGuid()
                  .WithColumn("Name").AsString(100).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventTagMappings")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("AltId").AsGuid()
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("TagId").AsInt64().ForeignKey("Tags", "Id")
                 .WithColumn("SortOrder").AsInt64().Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
