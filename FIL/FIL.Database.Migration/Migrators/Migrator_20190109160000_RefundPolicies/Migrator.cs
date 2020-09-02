using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190109160000_RefundPolicies
{

    [Migration(2019, 01, 09, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("RefundPolicies")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("policy").AsString(int.MaxValue).Nullable()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
