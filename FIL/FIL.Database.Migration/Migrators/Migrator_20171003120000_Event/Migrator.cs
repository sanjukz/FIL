using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20171003120000_Event
{
    [Migration(2017, 10, 03, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("TicketFeeDetails")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventTicketAttributeId").AsInt64().ForeignKey("EventTicketAttributes", "Id")
                .WithColumn("DiscountId").AsInt32().ForeignKey("Discounts", "Id").Nullable()
                .WithColumn("DisplayName").AsString(200).Nullable()
                .WithColumn("DiscountValueTypeId").AsInt16().ForeignKey("DiscountValueTypes", "Id")
                .WithColumn("Value").AsDecimal()
                .WithColumn("DiscountGroupId").AsInt16().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventTicketDiscountDetails")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventTicketAttributeId").AsInt64().ForeignKey("EventAttributes", "Id")
                .WithColumn("DiscountId").AsInt32().ForeignKey("Discounts", "Id")
                .WithColumn("StartDateTime").AsDateTime()
                .WithColumn("EndDateTime").AsDateTime()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventPaymentGateways")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("PaymentOptionGatewayId").AsInt32().ForeignKey("GatewayPaymentOptionMappings", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
