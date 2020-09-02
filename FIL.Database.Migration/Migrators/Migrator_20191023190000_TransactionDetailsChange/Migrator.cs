using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20191023190000_TransactionDetailsChange
{
    [Migration(2019, 10, 23, 19, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("transactiondetails").Column("VisitDate").Exists())
            {
                Alter.Table("transactiondetails").AddColumn("VisitDate").AsDateTime().Nullable();
            }
            if (!Schema.Table("transactiondetails").Column("IsRedeemed").Exists())
            {
                Alter.Table("transactiondetails").AddColumn("IsRedeemed").AsBoolean().Nullable();
            }
        }
    }
}
