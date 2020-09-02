using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190422120000_PartialVoidRequests
{
    [Migration(2019, 04, 21, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("PartialVoidRequestDetails").Exists())
            {
                Create.Table("PartialVoidRequestDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("BarcodeNumber").AsString(80)
                .WithColumn("RequestDateTimeUTC").AsDateTime()
                .WithColumn("IsPartialVoid").AsBoolean()
                .WithColumn("PartialVoidDateTime").AsDateTime()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable()
                .WithColumn("Reason").AsString(80);
            }
            if (!Schema.Table("BoxofficeUserAdditionalDetails").Column("CountryId").Exists())
            {
                Alter.Table("BoxofficeUserAdditionalDetails").AddColumn("CountryId").AsInt64().ForeignKey("Countries", "Id").Nullable();
            }
            if (!Schema.Table("TicketRefundDetails").Exists())
            {
                Create.Table("TicketRefundDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("BarcodeNumber").AsString(80)
                .WithColumn("TransactionId ").AsInt64()
                .WithColumn("RefundedAmount ").AsDecimal()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable()
                .WithColumn("ExchangedAmount ").AsDecimal()
                .WithColumn("ExchangedId ").AsInt64()
                .WithColumn("ActionTypeId ").AsInt16()
                .WithColumn("IsExchanged").AsBoolean();
            }
        }
    }
}
