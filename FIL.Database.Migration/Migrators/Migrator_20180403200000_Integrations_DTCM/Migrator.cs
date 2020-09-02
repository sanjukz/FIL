using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180403200000_Integrations_DTCM
{
    [Migration(2018, 04, 03, 20, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("DTCMEventTicketDetailMapping")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("EventTicketDetailId").AsInt64().ForeignKey("EventTicketDetails", "Id")
                .WithColumn("PriceCategoryId").AsInt16()
                .WithColumn("PriceCategoryCode").AsString(10).Nullable()
                .WithColumn("PriceCategoryName").AsString(100).Nullable()
                .WithColumn("PriceTypeId").AsInt16()
                .WithColumn("PriceTypeCode").AsString(10)
                .WithColumn("PriceTypeName").AsString(100).Nullable()
                .WithColumn("PriceTypeArea").AsString(20)
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("DTCMTransactionMapping")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("OrderId").AsString(100)
                .WithColumn("BasketId").AsString(100).Nullable()
                .WithColumn("BasketAmount").AsString(100).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("DTCMTransactionBarcodes")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("DTCMTransactionMapId").AsInt64().ForeignKey("DTCMTransactionMapping", "Id")
                .WithColumn("BarCode").AsString(100)
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
