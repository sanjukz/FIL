using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20200130200000_POne
{
    [Migration(2020, 01, 30, 20, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up() 
        {
            Create.Table("POneEvents")
                  .WithColumn("Id").AsInt32().Identity()
                  .WithColumn("POneId").AsInt32().PrimaryKey()
                  .WithColumn("Name").AsString(200)
                  .WithColumn("Description").AsString(int.MaxValue)
                  .WithColumn("TermsAndConditions").AsString(500)
                  .WithColumn("POneEventCategoryId").AsInt32()
                  .WithColumn("POneEventSubCategoryId").AsInt32()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("POneEventCategories")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Name").AsString(200)
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("POneEventSubCategories")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Name").AsString(200)
                  .WithColumn("POneEventCategoryId").AsInt32().ForeignKey("POneEventCategories", "Id")
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("POneVenues")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Name").AsString(500)
                  .WithColumn("Address").AsString(500)
                  .WithColumn("CityId").AsInt32()
                  .WithColumn("Latitude").AsString(200).Nullable()
                  .WithColumn("Longitude").AsString(200).Nullable()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("POneEventDetails")
                  .WithColumn("Id").AsInt32().Identity()
                  .WithColumn("POneId").AsInt32().PrimaryKey()
                  .WithColumn("Name").AsString(500)
                  .WithColumn("POneEventId").AsInt32().ForeignKey("POneEvents", "POneId")
                  .WithColumn("POneVenueId").AsInt32().ForeignKey("POneVenues", "Id")
                  .WithColumn("StartDateTime").AsDateTime()
                  .WithColumn("MetaDetails").AsString(int.MaxValue)
                  .WithColumn("Description").AsString(int.MaxValue)
                  .WithColumn("DeliveryTypeId").AsInt16()
                  .WithColumn("DeliveryNotes").AsString(int.MaxValue).Nullable()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("POneTicketCategories")
                  .WithColumn("Id").AsInt32().Identity()
                  .WithColumn("POneId").AsInt32().PrimaryKey()
                  .WithColumn("Name").AsString(500)
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("POneEventTicketDetails")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("POneTicketCategoryId").AsInt32().ForeignKey("POneTicketCategories", "POneId")
                  .WithColumn("POneEventDetailId").AsInt32().ForeignKey("POneEventDetails", "POneId")
                  .WithColumn("ImageUrl").AsString(500).Nullable()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("POneEventTicketAttributes")
                  .WithColumn("Id").AsInt64().Identity()
                  .WithColumn("POneId").AsInt64().PrimaryKey()
                  .WithColumn("POneEventTicketDetailId").AsInt32().ForeignKey("POneEventTicketDetails", "Id")
                  .WithColumn("AvailableTicketForSale").AsInt32()
                  .WithColumn("TicketCategoryDescription").AsString(int.MaxValue)
                  .WithColumn("IsDateConfirmed").AsBoolean()
                  .WithColumn("Price").AsDecimal(18, 2)
                  .WithColumn("ShippingCharge").AsDecimal(18, 2)
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("POneEventDetailMappings")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().Identity()
                  .WithColumn("POneEventDetailId").AsInt32().ForeignKey("POneEventDetails", "POneId")
                  .WithColumn("ZoongaEventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("POneTransactionDetails")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().Identity()
                  .WithColumn("TransactionId").AsInt64()
                  .WithColumn("POneOrderId").AsInt64()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("POneEventTicketAttributeMappings")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().Identity()
                  .WithColumn("POneEventTicketAttributeId").AsInt64().ForeignKey("POneEventTicketAttributes", "POneId")
                  .WithColumn("ZoongaEventTicketAttributeId").AsInt64().ForeignKey("EventTicketAttributes", "Id")
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            // Creating indexes --------------------------
            Create.Index().OnTable("POneEventDetailMappings")
                .OnColumn("POneEventDetailId").Ascending()
                .OnColumn("ZoongaEventDetailId").Ascending();

            Create.Index().OnTable("POneEventTicketAttributes")
                .OnColumn("POneId").Ascending();

            Create.Index().OnTable("POneEventDetails")
                .OnColumn("POneId").Ascending();
        } 
    }
}
