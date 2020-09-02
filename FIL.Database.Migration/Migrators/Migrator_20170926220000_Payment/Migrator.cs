using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20170926220000_Payment
{
    [Migration(2017, 09, 26, 22, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("DomesticCardBins")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PaymentOptionId").AsInt16().ForeignKey("PaymentOptions", "Id")
                .WithColumn("FirstSix").AsInt16()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("PaymentOptionDetails")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100)
                .WithColumn("PaymentOptionId").AsInt16().ForeignKey("PaymentOptions", "Id")
                .WithColumn("IsFrequentlyUsed").AsBoolean().Nullable()                
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("GatewayPaymentOptionMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PaymentOptionId").AsInt16().ForeignKey("PaymentOptions", "Id")
                .WithColumn("PaymentGatewayId").AsInt16().ForeignKey("PaymentGateways", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
