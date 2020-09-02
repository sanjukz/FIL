using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180906223000_Optimizations_Indexes
{
    [Migration(2018, 09, 06, 22, 30, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Users").Index("idx_Users_Email").Exists())
            {
                Create.Index()
                    .OnTable("Users")
                    .OnColumn("Email");
            }

            if (!Schema.Table("MatchSeatTicketDetails").Index("idx_MatchSeatTicketDetails_TransactionId_MatchLayoutSectionSeatId_EventTicketDetailId_BarcodeNumber").Exists())
            {
                Create.Index()
                    .OnTable("MatchSeatTicketDetails")
                    .OnColumn("TransactionId").Ascending()
                    .OnColumn("MatchLayoutSectionSeatId").Ascending()
                    .OnColumn("EventTicketDetailId").Ascending()
                    .OnColumn("BarcodeNumber").Ascending();
            }

            if (!Schema.Table("Transactions").Index("idx_Transactions_AltId_EmailId_PhoneNumber").Exists())
            {
                Create.Index()
                    .OnTable("Transactions")
                    .OnColumn("AltId").Ascending()
                    .OnColumn("EmailId").Ascending()
                    .OnColumn("PhoneNumber").Ascending();
            }

        }
    }
}
