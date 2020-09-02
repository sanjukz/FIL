using System;
using System.Collections.Generic;
using System.Text;
using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190214130000_CitySightSeeing
{
    [Migration(2019, 02, 14, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("CitySightSeeingLocations").Exists())
            {
                Create.Table("CitySightSeeingLocations")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("Name").AsString(100).Nullable()
                    .WithColumn("CountryName").AsString(100).Nullable()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("CitySightSeeingTickets").Exists())
            {
                Create.Table("CitySightSeeingTickets")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("TicketId").AsString(100).Nullable()
                    .WithColumn("Title").AsString(100).Nullable()
                    .WithColumn("VenueName").AsString(500).Nullable()
                    .WithColumn("Language").AsString(500).Nullable()
                    .WithColumn("StartDate").AsString(500).Nullable()
                    .WithColumn("EndDate").AsString(500).Nullable()
                    .WithColumn("Currency").AsString(500).Nullable()
                    .WithColumn("CitySightSeeingLocationId").AsInt32().ForeignKey("CitySightSeeingLocations", "Id")
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("CitySightSeeingCompanyOpeningTimes").Exists())
            {
                Create.Table("CitySightSeeingCompanyOpeningTimes")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("TicketId").AsString(100).Nullable()
                    .WithColumn("Day").AsString(100).Nullable()
                    .WithColumn("StartFrom").AsString(500).Nullable()
                    .WithColumn("EndTo").AsString(500).Nullable()
                    .WithColumn("CitySightSeeingTicketId").AsInt32().ForeignKey("CitySightSeeingTickets", "Id")
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("CitySightSeeingTicketTypeDetails").Exists())
            {
                Create.Table("CitySightSeeingTicketTypeDetails")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("TicketId").AsString(100).Nullable()
                    .WithColumn("TicketType").AsString(100).Nullable()
                    .WithColumn("StartDate").AsString(500).Nullable()
                    .WithColumn("EndDate").AsString(500).Nullable()
                    .WithColumn("AgeFrom").AsInt32()
                    .WithColumn("AgeTo").AsInt32()
                    .WithColumn("UnitPrice").AsString(100).Nullable()
                    .WithColumn("UnitListPrice").AsString(100).Nullable()
                    .WithColumn("UnitDiscount").AsString(100).Nullable()
                    .WithColumn("UnitGrossPrice").AsString(100).Nullable()
                    .WithColumn("CitySightSeeingTicketId").AsInt32().ForeignKey("CitySightSeeingTickets", "Id")
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("CitySightSeeingTicketDetails").Exists())
            {
                Create.Table("CitySightSeeingTicketDetails")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("TicketId").AsString(100).Nullable()
                    .WithColumn("Title").AsString(100).Nullable()
                    .WithColumn("ShortDescription").AsString(500).Nullable()
                    .WithColumn("Description").AsString(int.MaxValue).Nullable()
                    .WithColumn("Duration").AsString(500).Nullable()
                    .WithColumn("ProductLanguage").AsString(500).Nullable()
                    .WithColumn("TxtLanguage").AsString(500).Nullable()
                    .WithColumn("TicketEntryNotes").AsString(500).Nullable()                    
                    .WithColumn("BookSizeMin").AsString(500).Nullable()
                    .WithColumn("BookSizeMax").AsString(500).Nullable()
                    .WithColumn("SupplierUrl").AsString(500).Nullable()
                    .WithColumn("TicketClass").AsInt32()
                    .WithColumn("StartDate").AsString(500).Nullable()
                    .WithColumn("EndDate").AsString(500).Nullable()
                    .WithColumn("BookingStartDate").AsString(500).Nullable()
                    .WithColumn("Currency").AsString(500).Nullable()
                    .WithColumn("PickupPoints").AsString(500).Nullable()
                    .WithColumn("CombiTicket").AsString(500).Nullable()
                    .WithColumn("CitySightSeeingTicketId").AsInt32().ForeignKey("CitySightSeeingTickets", "Id")
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("CitySightSeeingTicketDetailImages").Exists())
            {
                Create.Table("CitySightSeeingTicketDetailImages")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("TicketId").AsString(100).Nullable()
                    .WithColumn("Image").AsString(500).Nullable()
                    .WithColumn("CitySightSeeingTicketId").AsInt32().ForeignKey("CitySightSeeingTickets", "Id")
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

        }
    }
}
