using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20191102140000_StateDescription
{
    [Migration(2019, 11, 02, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("StateDescriptions")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("StateId").AsInt32().ForeignKey("states", "Id")
                  .WithColumn("Description").AsString(100).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
