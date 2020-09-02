using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20170927120000_Event
{
    [Migration(2017, 09, 27, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Keywords")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Keywords").AsString(int.MaxValue)
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TicketCategories")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(200)
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ClientPointOfContacts")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("Name").AsString(20)
                .WithColumn("Email").AsString(128)
                .WithColumn("PhoneNumber").AsString(10)
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Events")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("EventCategoryId").AsInt16().ForeignKey("EventCategories", "Id")
                .WithColumn("EventTypeId").AsInt16().ForeignKey("EventTypes", "Id")
                .WithColumn("Name").AsString(100)
                .WithColumn("Description").AsString(int.MaxValue).Nullable()
                .WithColumn("ClientPointOfContactId").AsInt32().ForeignKey("ClientPointOfContacts", "Id")
                .WithColumn("FbEventId").AsInt64().Nullable()
                .WithColumn("MetaDetails").AsString(int.MaxValue).Nullable()
                .WithColumn("TermsAndConditions").AsString(int.MaxValue)
                .WithColumn("IsFeel").AsBoolean()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("IsPublishedOnSite").AsBoolean().Nullable()
                .WithColumn("PublishedDateTime").AsDateTime().Nullable()
                .WithColumn("PublishedBy").AsInt32().Nullable()
                .WithColumn("TestedBy").AsInt32().Nullable()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventKeywords")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("SearchKeywordId").AsInt32().ForeignKey("Keywords", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100)
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("VenueId").AsInt32().ForeignKey("Venues", "Id")
                .WithColumn("StartDateTime").AsDateTime()
                .WithColumn("EndDateTime").AsDateTime()
                .WithColumn("GroupId").AsInt64()
                .WithColumn("MetaDetails").AsString(int.MaxValue).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("HideEventDateTime").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CustomDateTimeMessage").AsString(500)
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventDeliveryTypeDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
                .WithColumn("DeliveryTypeId").AsInt16().ForeignKey("DeliveryTypes", "Id")
                .WithColumn("Notes").AsString(int.MaxValue)
                .WithColumn("EndDate").AsDateTime()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventAttributes")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
                .WithColumn("MatchNo").AsInt16().Nullable()
                .WithColumn("MatchDay").AsInt16().Nullable()                
                .WithColumn("GateOpenTime").AsString(20).Nullable()
                .WithColumn("TimeZone").AsString(20).Nullable()
                .WithColumn("TimeZoneAbbreviation").AsString(20).Nullable()
                .WithColumn("TicketHtml").AsString(20).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("FeaturedEvents")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("SortOrder").AsInt16()
                .WithColumn("SiteId").AsInt16().ForeignKey("Sites", "Id")
                .WithColumn("IsAllowedInFooter").AsBoolean().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventFacebookReservations")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("FbUserId").AsInt64()
                .WithColumn("FbUserName").AsString(20)
                .WithColumn("FbProfileUrl").AsString(20).Nullable()
                .WithColumn("FbProfilePicUrl").AsString(20).Nullable()
                .WithColumn("FbUserEmail").AsString(128).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventTicketDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
                .WithColumn("TicketCategoryId").AsInt32().ForeignKey("TicketCategories", "Id")
                //.WithColumn("AvailableTicketForSale").AsInt16()
                //.WithColumn("RemainingTicketForSale").AsInt16()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventTicketAttributes")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("EventTicketDetailId").AsInt64().ForeignKey("EventTicketDetails", "Id")
                .WithColumn("SalesStartDateTime").AsDateTime()
                .WithColumn("SalesEndDatetime").AsDateTime()
                .WithColumn("TicketTypeId").AsInt16().ForeignKey("TicketTypes", "Id")
                .WithColumn("ChannelId").AsInt16().ForeignKey("Channels", "Id")
                .WithColumn("CurrencyId").AsInt32().ForeignKey("CurrencyTypes", "Id")
                .WithColumn("SharedInventoryGroupId").AsInt16().Nullable()
                .WithColumn("AvailableTicketForSale").AsInt16()
                .WithColumn("RemainingTicketForSale").AsInt16()
                .WithColumn("TicketCategoryDescription").AsString(200)
                .WithColumn("ViewFromStand").AsString(20)
                .WithColumn("IsSeatSelection").AsBoolean()
                .WithColumn("Price").AsDecimal(18, 2)
                .WithColumn("IsInternationalCardAllowed").AsBoolean()
                .WithColumn("IsEMIApplicable").AsBoolean()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
