using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190904140000_RASVMemberId
{
    [Migration(2019, 09, 04, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("TransactionDetails").Column("MembershipId").Exists())
            {
              Alter.Table("TransactionDetails")
             .AddColumn("MembershipId")
             .AsString(200)
             .Nullable();
            }           
        }
    }
}
