using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180102210000_Corporate
{
    [Migration(2018, 01, 02, 21, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("CorporateRequests")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("SponsorName").AsString(50)
                .WithColumn("FirstName").AsString(20)
                .WithColumn("LastName").AsString(20)
                .WithColumn("Email").AsString(128)
                .WithColumn("PhoneCode").AsString(20)
                .WithColumn("PhoneNumber").AsString(20)
                .WithColumn("Address").AsString(200)
                .WithColumn("ZipcodeId").AsInt32().ForeignKey("Zipcodes", "Id")
                .WithColumn("PickupRepresentativeFirstName").AsString(20)
                .WithColumn("PickupRepresentativeLastName").AsString(20)
                .WithColumn("PickupRepresentativeEmail").AsString(128)
                .WithColumn("PickupRepresentativePhoneCode").AsString(20)
                .WithColumn("PickupRepresentativePhoneNumber").AsString(20)
                .WithColumn("SponsorId").AsInt64().ForeignKey("Sponsors", "Id").Nullable()
                .WithColumn("RequestOrderType").AsBoolean().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("CorporateRequestDetails")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("CorporateRequestId").AsInt64().ForeignKey("CorporateRequests", "Id")
               .WithColumn("EventTicketDetailId").AsInt64().ForeignKey("EventTicketDetails", "Id")
               .WithColumn("RequestedTickets").AsInt16().Nullable()
               .WithColumn("ApprovedTickets").AsInt16().Nullable()
               .WithColumn("Price").AsDecimal(18, 2)
               .WithColumn("ApprovedStatus").AsBoolean()
               .WithColumn("ApprovedBy").AsGuid().Nullable()
               .WithColumn("ApprovedUtc").AsDateTime().Nullable()
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("BankDetails")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("BankDetails").AsString(int.MaxValue)
               .WithColumn("CountryId").AsInt32().ForeignKey("Countries", "Id")
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("InvoiceDetails")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("InvoiceNumber").AsString(50)
               .WithColumn("CorporateRequestId").AsInt64().ForeignKey("CorporateRequests", "Id")
               .WithColumn("EventTicketDetailId").AsInt64().ForeignKey("EventTicketDetails", "Id")
               .WithColumn("TotalTickets").AsInt16().Nullable()
               .WithColumn("Price").AsDecimal(18, 2)
               .WithColumn("ConvenienceCharges").AsDecimal(18, 2).Nullable()
               .WithColumn("ServiceCharge").AsDecimal(18, 2).Nullable()
               .WithColumn("VAT").AsDecimal(18, 2).Nullable()
               .WithColumn("DiscountAmount").AsDecimal(18, 2).Nullable()
               .WithColumn("GrossTicketAmount").AsDecimal(18, 2).Nullable()
               .WithColumn("NetTicketAmount").AsDecimal(18, 2).Nullable()
               .WithColumn("BankDetailsId").AsInt64().ForeignKey("BankDetails", "Id")
               .WithColumn("InvoiceGeneratedBy").AsGuid().Nullable()
               .WithColumn("InvoiceGeneratedUtc").AsDateTime().Nullable()
               .WithColumn("InvoiceSentToEmailId").AsString(int.MaxValue)
               .WithColumn("InvoiceSentCCEmailId").AsString(int.MaxValue)
               .WithColumn("InvoiceSentBCCEmailId").AsString(int.MaxValue)
               .WithColumn("InvoiceSentDate").AsDateTime().Nullable()
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
   