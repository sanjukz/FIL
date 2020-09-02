using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190905180000_BOCustomerZipcode
{
    [Migration(2019, 09, 05, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("BoCustomerDetails").Column("ZipCode").Exists())
            {
                Alter.Table("BoCustomerDetails")
               .AddColumn("ZipCode")
               .AsString(100)
               .Nullable();
            }
        }
    }
}
