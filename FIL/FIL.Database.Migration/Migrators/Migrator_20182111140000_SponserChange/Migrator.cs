using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20182111140000_SponserChange
{
    [Migration(2018, 21, 11, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Sponsors").Column("Password").Exists())
            {
                Alter.Table("Sponsors").AddColumn("Password").AsString(int.MaxValue).Nullable();
            }
        }
    }
}
