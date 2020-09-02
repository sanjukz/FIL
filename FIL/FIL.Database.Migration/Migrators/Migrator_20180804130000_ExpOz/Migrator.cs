using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180804130000_ExpOz
{
    [Migration(2018, 08, 04, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Events").Column("IsExpOz").Exists())
            {
                Alter.Table("Events")
                .AddColumn("IsExpOz")
                .AsBoolean().WithDefaultValue(false);
            }
        }
    }
}