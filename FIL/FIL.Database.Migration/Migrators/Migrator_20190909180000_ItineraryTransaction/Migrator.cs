using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190909180000_ItineraryTransaction
{
    [Migration(2019, 09, 09, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("TransactionDetails").Column("VisitEndDate").Exists())
            {
                Alter.Table("TransactionDetails")
               .AddColumn("VisitEndDate")
               .AsDateTime()
               .Nullable();
            }

            if (!Schema.Table("TransactionDetails").Column("TransactionType").Exists())
            {
                Alter.Table("TransactionDetails")
               .AddColumn("TransactionType").AsInt16().ForeignKey("TransactionTypes", "Id")
               .Nullable();
            }
        }
    }
}
