using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20171016103000_Transaction
{
    [Migration(2017, 10, 16, 10, 30, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("TransactionSeatDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("TransactionDetailId").AsInt64().ForeignKey("TransactionDetails", "Id")
                .WithColumn("MatchSeatTicketDetailId").AsInt64().ForeignKey("MatchSeatTicketDetails", "Id")
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TransactionPaymentDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("PaymentOptionId").AsInt16().ForeignKey("PaymentOptions", "Id").Nullable()
                .WithColumn("PaymentGatewayId").AsInt16().ForeignKey("PaymentGateways", "Id").Nullable()
                .WithColumn("UserCardDetailId").AsInt32().ForeignKey("CardDetails", "Id").Nullable()
                .WithColumn("RequestType").AsString(100)
                .WithColumn("Amount").AsString(100)
                .WithColumn("PayConfNumber").AsString(100).Nullable()
                .WithColumn("PaymentDetail").AsString(int.MaxValue).Nullable()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TransactionPromoCodes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("DiscountPromoCodeId").AsInt32().ForeignKey("DiscountPromoCodes", "Id")
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
