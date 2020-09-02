using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190114120000_EventCategories
{
    [Migration(2019, 01, 14, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("EventCategories").Column("IsEnabled").Exists())
            {
                Alter.Table("EventCategories")
                    .AddColumn("IsEnabled").AsBoolean().Nullable()
                    .AddColumn("CreatedUtc").AsDateTime().Nullable()
                    .AddColumn("UpdatedUtc").AsDateTime().Nullable()
                    .AddColumn("CreatedBy").AsGuid().Nullable()
                    .AddColumn("UpdatedBy").AsGuid().Nullable();
            }            
        }
    }
}
