using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190725140000_ValueRetail
{
    [Migration(2019, 07, 25, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("ValueRetailExpressRoutes").Exists())
            {
                Create.Table("ValueRetailExpressRoutes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("VillageCode").AsString(50)
                .WithColumn("JourneyType").AsInt32()
                .WithColumn("RouteId").AsInt32()
                .WithColumn("Name").AsString(100)
                .WithColumn("StopId").AsInt32()
                .WithColumn("DepartureTime").AsString(100)
                .WithColumn("LocationName").AsString(100)
                .WithColumn("LocationAddress").AsString(100)
                .WithColumn("Latitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("Longitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("AdultPrice").AsDecimal(18, 2).NotNullable()
                .WithColumn("ChildrenPrice").AsDecimal(18, 2).NotNullable()
                .WithColumn("FamilyPrice").AsDecimal(18, 2).NotNullable()
                .WithColumn("InfantPrice").AsDecimal(18, 2).NotNullable()
                .WithColumn("UnitPrice").AsDecimal(18, 2).NotNullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("ValueRetailVillages").Exists())
            {
                Create.Table("ValueRetailVillages")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("VillageCode").AsString(50)
                .WithColumn("CultureCode").AsString(50)
                .WithColumn("CurrencyCode").AsString(50)
                .WithColumn("VillageName").AsString(50)
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("EventVenueMappings").Exists())
            {
                Create.Table("EventVenueMappings")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("EventId").AsInt64()
                    .WithColumn("VenueId").AsInt32()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("EventVenueMappingTimes").Exists())
            {
                Create.Table("EventVenueMappingTimes")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("EventVenueMappingId").AsInt32().ForeignKey("EventVenueMappings", "Id")
                    .WithColumn("PickupTime").AsString(50).Nullable()
                    .WithColumn("PickupLocation").AsString(500).Nullable()
                    .WithColumn("ReturnTime").AsString(50).Nullable()
                    .WithColumn("ReturnLocation").AsString(500).Nullable()
                    .WithColumn("JourneyType").AsInt32()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("ValueRetailBookingDetails").Exists())
            {
                Create.Table("ValueRetailBookingDetails")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("JobId").AsInt32()
                    .WithColumn("Email").AsString(500)
                    .WithColumn("Date").AsDateTime().Nullable()
                    .WithColumn("VillageCode").AsString(50).Nullable()
                    .WithColumn("CultureCode").AsString(50).Nullable()
                    .WithColumn("Pricing").AsDecimal(18, 2).NotNullable()
                    .WithColumn("ValueRetailAltId").AsString(100)
                    .WithColumn("ReferenceURL").AsString(100)
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("ValueRetailRoutes").Exists())
            {
                Create.Table("ValueRetailRoutes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("VillageId").AsInt32()
                .WithColumn("JourneyType").AsInt32()
                .WithColumn("RouteId").AsInt32()
                .WithColumn("DepartureTime").AsString(50)
                .WithColumn("LinkedRouteId").AsInt32()
                .WithColumn("ReturnTime").AsString(50)
                .WithColumn("Name").AsString(100)
                .WithColumn("LocationId").AsInt32()
                .WithColumn("LocationName").AsString(100)
                .WithColumn("LocationAddress").AsString(100)
                .WithColumn("StopId").AsInt32()
                .WithColumn("StopOrder").AsInt32()
                .WithColumn("Latitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("Longitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("TransactionMoveAroundMappings").Exists())
            {
                Create.Table("TransactionMoveAroundMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("EventVenueMappingTimeId").AsInt32().ForeignKey("EventVenueMappingTimes", "Id")
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    }
}