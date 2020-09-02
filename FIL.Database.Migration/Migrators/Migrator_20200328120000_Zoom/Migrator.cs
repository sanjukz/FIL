using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20200328120000_Zoom
{
    [Migration(2020, 03, 28, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {

            Create.Table("Services")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Name").AsString(int.MaxValue)
                  .WithColumn("Description").AsString(int.MaxValue).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Languages")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Code").AsString(200).Nullable()
                  .WithColumn("Name").AsString(200).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Redemption_GuideDetails")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                  .WithColumn("UserAddressDetailId").AsInt64().ForeignKey("useraddressdetails", "Id")
                  .WithColumn("LanguageId").AsString(200).Nullable()
                  .WithColumn("ApproveStatusId").AsInt16().ForeignKey("ApproveStatuses", "Id")
                  .WithColumn("ApprovedBy").AsGuid().Nullable()
                  .WithColumn("ApprovedUtc").AsDateTime().Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Redemption_GuidePlaceMappings")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("GuideId").AsInt64().ForeignKey("Redemption_GuideDetails", "Id")
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("ApproveStatusId").AsInt16().ForeignKey("ApproveStatuses", "Id")
                 .WithColumn("ApprovedBy").AsGuid().Nullable()
                 .WithColumn("ApprovedUtc").AsDateTime().Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Redemption_GuideServices")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("GuideId").AsInt64().ForeignKey("Redemption_GuideDetails", "Id")
                 .WithColumn("ServiceId").AsInt32().ForeignKey("Services", "Id")
                 .WithColumn("Notes").AsString(int.MaxValue).Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("MasterFinanceDetails")
                 .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                 .WithColumn("CurrenyId").AsInt32().ForeignKey("CurrencyTypes", "Id")
                 .WithColumn("CountryId").AsInt32().ForeignKey("Countries", "Id")
                 .WithColumn("StateId").AsInt32().ForeignKey("States", "Id")
                 .WithColumn("AccountTypeId").AsInt16().ForeignKey("AccountTypes", "Id")
                 .WithColumn("BankAccountTypeId").AsInt16().ForeignKey("BankAccountTypes", "Id")
                 .WithColumn("BankName").AsString(400).Nullable()
                 .WithColumn("AccountNumber").AsString(400).Nullable()
                 .WithColumn("BranchCode").AsString(400).Nullable()
                 .WithColumn("TaxId").AsString(400).Nullable()
                 .WithColumn("RoutingNumber").AsString(400).Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Redemption_CountryTaxTypeMappings")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("CountryId").AsInt32().ForeignKey("Countries", "Id")
                  .WithColumn("Name").AsString(400).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Redemption_GuideDocumentMappings")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("GuideId").AsInt64().ForeignKey("Redemption_GuideDetails", "Id")
                  .WithColumn("DocumentID").AsInt64().ForeignKey("CustomerDocumentTypes", "Id").Nullable()
                  .WithColumn("DocumentSouceURL").AsString(int.MaxValue).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Redemption_GuideFinanceMappings")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("GuideId").AsInt64().ForeignKey("Redemption_GuideDetails", "Id")
               .WithColumn("MasterFinanceDetailId").AsInt32().ForeignKey("MasterFinanceDetails", "Id")
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Redemption_OrderDetails")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity().Identity()
                  .WithColumn("AltId").AsGuid()
                  .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                  .WithColumn("OrderStatusId").AsInt16().ForeignKey("ApproveStatuses", "Id")
                  .WithColumn("ApprovedBy").AsGuid().Nullable()
                  .WithColumn("ApprovedUtc").AsDateTime().Nullable()
                  .WithColumn("OrderCompletedDate").AsDateTime().Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Blogs")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("BlogId").AsInt32()
                 .WithColumn("ImageUrl").AsString(1000).Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            // Creating indexes --------------------------
            Create.Index().OnTable("Redemption_GuideDetails")
                .OnColumn("UserId").Ascending();

            Create.Index().OnTable("Redemption_GuideDetails")
                .OnColumn("VenueId").Ascending();

            Create.Index().OnTable("Redemption_GuidePlaceMappings")
                .OnColumn("GuideId").Ascending();

            Create.Index().OnTable("Redemption_GuidePlaceMappings")
                .OnColumn("EventId").Ascending();

            Create.Index().OnTable("Redemption_GuidePlaceMappings")
                .OnColumn("GuideId").Ascending()
                .OnColumn("EventId").Ascending();

            Create.Index().OnTable("Redemption_OrderDetails")
                .OnColumn("TransactionId").Ascending();

            Create.Index().OnTable("Redemption_GuideServices")
                .OnColumn("IsEnabled").Ascending();

            Create.Index().OnTable("Redemption_GuidePlaceMappings")
                .OnColumn("IsEnabled").Ascending();

            Create.Index().OnTable("Redemption_GuideDetails")
               .OnColumn("IsEnabled").Ascending();

            Create.Index().OnTable("Redemption_GuideFinanceMappings")
              .OnColumn("GuideId").Ascending();

            Create.Index().OnTable("Redemption_GuideFinanceMappings")
               .OnColumn("MasterFinanceDetailId").Ascending();
        }
    }
}
