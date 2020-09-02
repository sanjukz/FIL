using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20200521180000_CompanyDetails
{
    [Migration(2020, 07, 02, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("CompanyDetails")
                 .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                 .WithColumn("Name").AsString(100)
                 .WithColumn("Address").AsString(100)
                 .WithColumn("Prefix").AsString(100)
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            if (Schema.Table("BankDetails").Exists())
            {
                Alter.Table("BankDetails").AddColumn("CompanyDetailId").AsInt32().ForeignKey("CompanyDetails", "Id").Nullable();
            }

            Create.Table("CorporateInvoiceDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("CurrencyId").AsInt32().ForeignKey("CurrencyTypes", "Id")
                .WithColumn("CompanyDetailId").AsInt32().ForeignKey("CompanyDetails", "Id")
                .WithColumn("BankDetailId").AsInt64().ForeignKey("BankDetails", "Id")
                .WithColumn("InvoiceDate").AsDateTime()
                .WithColumn("InvoiceDueDate").AsDateTime()
                .WithColumn("Address").AsString(int.MaxValue)
                .WithColumn("CityId").AsInt32().ForeignKey("Cities", "Id")
                .WithColumn("StateId").AsInt32().ForeignKey("States", "Id")
                .WithColumn("CountryId").AsInt32().ForeignKey("Countries", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("CorporateOrderInvoiceMappings")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("CorporateInvoiceDetailId").AsInt64().ForeignKey("CorporateInvoiceDetails", "Id")
                .WithColumn("CorporateOrderRequestId").AsInt64().ForeignKey("CorporateOrderRequests", "Id")
                .WithColumn("Quantity").AsInt32()
                .WithColumn("UnitPrice").AsDecimal()
                .WithColumn("TotalTicketAmount").AsDecimal()
                .WithColumn("ConvenienceCharge").AsDecimal()
                .WithColumn("ServiceCharge").AsDecimal()
                .WithColumn("ValueAddedTax").AsDecimal()
                .WithColumn("DiscountAmount").AsDecimal()
                .WithColumn("NetTicketAmount").AsDecimal()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}