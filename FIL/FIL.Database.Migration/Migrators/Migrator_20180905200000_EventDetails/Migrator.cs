using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180905200000_EventDetails
{
    [Migration(2018, 09, 05, 20, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("EventDetails").Column("Description").Exists())
            {
                Alter.Table("EventDetails").AddColumn("Description").AsString(int.MaxValue).Nullable();
            }
        }
    }
}
