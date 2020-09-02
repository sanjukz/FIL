using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180911140000_RemoveTable
{
    [Migration(2018, 09, 11, 14, 0, 0)]
    public class RemoveTable : BaseMigrator
    {
        public override void Up()
        {
            if (Schema.Table("SponsorPaidTransactionPaymentDetails").Exists())
            {
                Delete.Table("SponsorPaidTransactionPaymentDetails");
            }

            if (Schema.Table("SponsorTransactionDetails").Exists())
            {
                Delete.Table("SponsorTransactionDetails");
            }            
        }
    }
}
