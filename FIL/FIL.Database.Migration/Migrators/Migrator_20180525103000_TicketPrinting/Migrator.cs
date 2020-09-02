using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180525103000_TicketPrinting
{
    [Migration(2018, 05, 25, 10, 3, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("CorporateTicketPrintingLogs")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
            .WithColumn("TicketPrintingOptionId").AsInt16().ForeignKey("TicketPrintingOptions", "Id")
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedBy").AsGuid().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("HandoverSheets")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
            .WithColumn("SerialStart").AsString(200)
            .WithColumn("SerialEnd").AsString(200)
            .WithColumn("TicketHandedBy").AsString(200)
            .WithColumn("TicketHandedTo").AsString(200)
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedBy").AsGuid().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
        }
    }
}
