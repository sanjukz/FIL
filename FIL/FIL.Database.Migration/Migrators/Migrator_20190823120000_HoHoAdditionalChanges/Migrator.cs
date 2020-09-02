using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.MigratorsMigrator_20190823120000_HoHoAdditionalChanges
{
    [Migration(2019, 08, 23, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("FeelBarcodeMappings").Column("GroupCodeExist").Exists())
            {
                Alter.Table("FeelBarcodeMappings").AddColumn("GroupCodeExist").AsBoolean().Nullable();
            }
        }
    }
}
