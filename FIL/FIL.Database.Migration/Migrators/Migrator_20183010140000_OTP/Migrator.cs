using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20183010140000_OTP
{
    [Migration(2018, 30, 10, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (Schema.Table("Transactions").Column("OTP").Exists())
            {
                Alter.Table("Transactions").AlterColumn("OTP").AsInt32().Nullable();
            }
        }
    }
}
