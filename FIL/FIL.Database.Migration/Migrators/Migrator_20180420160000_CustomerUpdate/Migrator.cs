using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180420160000_CustomerUpdate
{
    [Migration(2018, 04, 20, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("CustomerUpdates")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(int.MaxValue)
                .WithColumn("SortOrder").AsInt16()
                .WithColumn("SiteId").AsInt16().ForeignKey("Sites", "Id").Indexed()
                .WithColumn("IsEnabled").AsBoolean().Indexed()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
