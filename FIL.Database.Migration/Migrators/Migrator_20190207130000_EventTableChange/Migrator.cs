using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190207130000_EventTableChange
{
    [Migration(2019, 02, 07, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("events").Column("IsCreatedFromFeelAdmin").Exists())
            {
                Alter.Table("events").AddColumn("IsCreatedFromFeelAdmin").AsBoolean().Nullable();
            }
        }
    }
}
