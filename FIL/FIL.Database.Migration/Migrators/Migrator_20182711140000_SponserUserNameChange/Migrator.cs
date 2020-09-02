using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20182711140000_SponserUserNameChange
{
    [Migration(2018, 27, 11, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Sponsors").Column("Username").Exists())
            {
                Alter.Table("Sponsors").AddColumn("Username").AsString(int.MaxValue).Nullable();
            }
        }
    }
}
