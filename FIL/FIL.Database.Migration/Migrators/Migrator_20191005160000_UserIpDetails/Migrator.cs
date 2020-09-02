using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_Migrator_20191005160000_UserIpDetails
{
    [Migration(2019, 10, 05, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Users").Column("IPDetailId").Exists())
            {
                Alter.Table("Users")
                .AddColumn("IPDetailId")
                .AsInt32().ForeignKey("IPDetails", "Id")
                .Nullable();
            }
        }
    }
}
