using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180910160000_BoxOffice
{
    [Migration(2018, 09, 10, 16, 0, 0)]
    public class BoxOffice : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("BO_MenuMaster").Exists())
            {
                Create.Table("BO_MenuMaster")
                    .WithColumn("Menu_Id").AsInt64()
                    .WithColumn("ParentMenu_Id").AsInt64().Nullable()
                    .WithColumn("MenuName").AsString(50)
                    .WithColumn("DisplayOrder").AsInt32()
                    .WithColumn("EventCategory").AsInt64()
                    .WithColumn("URL").AsString(500).Nullable()
                    .WithColumn("PageName").AsString(100).Nullable()
                    .WithColumn("UpdateDate").AsDateTime()
                    .WithColumn("UpdatedBy").AsString(50).Nullable()
                    .WithColumn("status").AsInt32()
                    .WithColumn("IsRetail").AsInt32().Nullable()
                    .WithColumn("IsBO").AsInt32().Nullable()
                    .WithColumn("MenuUrl").AsString(2000).Nullable()
                    .WithColumn("MenuPrefix").AsString(int.MaxValue).Nullable();
            }
            if (!Schema.Table("BarcodeScanLogs").Exists())
            {
                Create.Table("BarcodeScanLogs")
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("DeviceName").AsString(1000).Nullable()
                    .WithColumn("BarcodeNumber").AsString(200).Nullable()
                    .WithColumn("ScanGateName").AsString(1000).Nullable()
                    .WithColumn("EventTicketDetailId").AsInt64().Nullable()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
            if (!Schema.Table("BO_FloatAmount").Exists())
            {
                Create.Table("BO_FloatAmount")
                    .WithColumn("PCash_Id").AsInt64().Identity().PrimaryKey()
                    .WithColumn("Retailer_id").AsInt64().Nullable()
                    .WithColumn("AmtIn(USD)").AsDecimal().Nullable()
                    .WithColumn("AmtIn(Local)").AsDecimal().Nullable()
                    .WithColumn("VenueId").AsInt64().Nullable()
                    .WithColumn("LocalCurrencyId").AsInt64()
                    .WithColumn("Createddate").AsDateTime().Nullable();
            }
            if (!Schema.Table("BO_RetailCustomer").Exists())
            {
                Create.Table("BO_RetailCustomer")
                    .WithColumn("Rc_Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("Trans_Id").AsInt64().Nullable()
                    .WithColumn("Retailer_Id").AsInt64()
                    .WithColumn("Cust_FirstName").AsString(100).Nullable()
                    .WithColumn("Cust_LastName").AsString(50).Nullable()
                    .WithColumn("Cust_Address").AsString(500).Nullable()
                    .WithColumn("Cust_City").AsString(60).Nullable()
                    .WithColumn("Cust_PinCode").AsString(50).Nullable()
                    .WithColumn("Cust_State").AsString(50).Nullable()
                    .WithColumn("Cust_Email").AsString(100).Nullable()
                    .WithColumn("Cust_PaymentMode ").AsInt32().Nullable()
                    .WithColumn("Cust_MobileNumber").AsString(50).Nullable()
                    .WithColumn("Cust_Event").AsString(100).Nullable()
                    .WithColumn("Cust_EventID").AsInt64().Nullable()
                    .WithColumn("OrderNumber").AsString(50).Nullable()
                    .WithColumn("Cust_IdType").AsString(100).Nullable()
                    .WithColumn("Cust_IdTypeNumber").AsString(100).Nullable()
                    .WithColumn("DateandTime").AsDateTime().Nullable()
                    .WithColumn("TicketCategory").AsString(50).Nullable()
                    .WithColumn("NoOfTic ").AsInt32().Nullable()
                    .WithColumn("TheaterName").AsString(500).Nullable()
                    .WithColumn("MovieName").AsString(500).Nullable()
                    .WithColumn("PricePerTic").AsString(50).Nullable()
                    .WithColumn("ShowDateTime").AsString(100).Nullable()
                    .WithColumn("ThCity").AsString(50).Nullable()
                    .WithColumn("UpdatedBy").AsString(50).Nullable()
                    .WithColumn("TotalCharge").AsDecimal().Nullable()
                    .WithColumn("ConvenienceCharge").AsDecimal().Nullable()
                    .WithColumn("ServiceTax").AsDecimal().Nullable()
                    .WithColumn("DealerAccessCharge").AsDecimal().Nullable()
                    .WithColumn("TotalAmountPayable").AsDecimal().Nullable()
                    .WithColumn("UpdateDate").AsDateTime().Nullable()
                    .WithColumn("Cust_Country").AsString(60).Nullable()
                    .WithColumn("IsVerified").AsBoolean().Nullable()
                    .WithColumn("Cust_Address2").AsString(500).Nullable()
                    .WithColumn("Cust_Address3").AsString(500).Nullable()
                    .WithColumn("Cust_Dob").AsString(50).Nullable()
                    .WithColumn("CompanyName").AsString(200).Nullable()
                    .WithColumn("PaymentType").AsString(1000).Nullable()
                    .WithColumn("Discount").AsString(50).Nullable()
                    .WithColumn("Discount_Comment").AsString(500).Nullable();
            }
            if (!Schema.Table("BO_StartandEndingSrNo").Exists())
            {
                Create.Table("BO_StartandEndingSrNo")
                    .WithColumn("Ticket_SrNoID").AsInt64().Identity()
                    .WithColumn("StartSrNo").AsString(500).Nullable()
                    .WithColumn("EndSrNo").AsString(500).Nullable()
                    .WithColumn("Retailer_id").AsInt64().Nullable()
                    .WithColumn("VenueId").AsInt64().Nullable()
                    .WithColumn("CreatedDate").AsDateTime().Nullable();
            }
            if (!Schema.Table("BO_TransactionRevertRequest").Exists())
            {
                Create.Table("BO_TransactionRevertRequest")
                    .WithColumn("ReuestId").AsInt64().Identity()
                    .WithColumn("TransId").AsInt64().Nullable()
                    .WithColumn("Retailer_id").AsInt64().Nullable()
                    .WithColumn("Comments").AsString(500).Nullable()
                    .WithColumn("IsRevert").AsBoolean().Nullable()
                    .WithColumn("ReqDateTime").AsDateTime().Nullable()
                    .WithColumn("RevertDate").AsDateTime().Nullable();
            }
            if (!Schema.Table("BO_UserMenuRights").Exists())
            {
                Create.Table("BO_UserMenuRights")
                    .WithColumn("User_Id").AsInt64().Identity()
                    .WithColumn("MenuId").AsInt64()
                    .WithColumn("status").AsInt32()
                    .WithColumn("UpdatedBy").AsString(50).Nullable()
                    .WithColumn("UpdateDate").AsDateTime().Nullable();
            }
            if (!Schema.Table("BoCustomerDetails").Exists())
            {
                Create.Table("BoCustomerDetails")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("AltId").AsGuid()
                  .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                  .WithColumn("PaymentMode").AsString(200)
                  .WithColumn("BankName").AsString(400).Nullable()
                  .WithColumn("ChequeNumber").AsString(80).Nullable()
                  .WithColumn("ChequeDate").AsDateTime().Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
            if (!Schema.Table("BoUserVenues").Exists())
            {
                Create.Table("BoUserVenues")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("AltId").AsGuid()
                  .WithColumn("UserId").AsInt64()
                  .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                  .WithColumn("VenueId").AsInt64()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
            if (!Schema.Table("BoxofficeUserAdditionalDetails").Exists())
            {
                Create.Table("BoxofficeUserAdditionalDetails")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("ParentId").AsInt64().Nullable()
                  .WithColumn("UserId").AsInt64()
                  .WithColumn("IsChildTicket").AsBoolean().Nullable()
                  .WithColumn("IsSrCitizenTicket").AsBoolean().Nullable()
                  .WithColumn("TicketLimit").AsInt16().Nullable()
                  .WithColumn("ChildTicketLimit").AsInt16().Nullable()
                  .WithColumn("ChildForPerson").AsInt16().Nullable()
                  .WithColumn("SrCitizenLimit").AsInt16().Nullable()
                  .WithColumn("SrCitizenPerson").AsInt16().Nullable()
                  .WithColumn("CityId").AsString(40).Nullable()
                  .WithColumn("Address").AsString(400).Nullable()
                  .WithColumn("ContactNumber").AsString(8).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable()
                  .WithColumn("UserType").AsInt32().Nullable();
            }
        }
    }
}
