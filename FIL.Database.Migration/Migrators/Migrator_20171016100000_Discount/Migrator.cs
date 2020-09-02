using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20171016100000_Discount
{
    [Migration(2017, 10, 16, 10, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("DiscountPromoCodes")
               .WithColumn("Id").AsInt32().PrimaryKey().Identity()
               .WithColumn("PromoCode").AsString(20)
               .WithColumn("DiscountId").AsInt32().ForeignKey("Discounts", "Id")
               .WithColumn("PromoCodeStatusId").AsInt16().ForeignKey("PromoCodeStatuses", "Id")
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
            
            Create.Table("DiscountPaymentOptions")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PaymentOptionId").AsInt16().ForeignKey("PaymentOptions", "Id")
                .WithColumn("DiscountId").AsInt32().ForeignKey("Discounts", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("DiscountCustomers")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CustomerEmail").AsString(128)
                .WithColumn("CustomerPhone").AsString(20)
                .WithColumn("DiscountCustomersTypeId").AsInt16().ForeignKey("DiscountCustomersTypes", "Id")
                .WithColumn("DiscountId").AsInt32().ForeignKey("Discounts", "Id")
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id").Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
