using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190821150000_ValueRetailAlter
{
    [Migration(2019, 08, 21, 15, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("ValueRetailReturnStops")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ValueRetailRouteId").AsInt32().ForeignKey("ValueRetailRoutes", "Id")
                .WithColumn("RouteId").AsInt32()
                .WithColumn("StopId").AsInt32()
                .WithColumn("StopOrder").AsInt32()
                .WithColumn("Name").AsString(200)
                .WithColumn("LocationId").AsInt32()
                .WithColumn("LocationName").AsString(200)
                .WithColumn("LocationAddress").AsString(200)
                .WithColumn("ReturnTime").AsString(50)
                .WithColumn("Latitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("Longitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ValueRetailPackageRoutes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("VillageId").AsInt32()
                .WithColumn("PackageId").AsInt32()
                .WithColumn("JourneyType").AsInt32()
                .WithColumn("RouteId").AsInt32()
                .WithColumn("DepartureTime").AsString(50).Nullable()
                .WithColumn("LinkedRouteId").AsInt32()
                .WithColumn("ReturnTime").AsString(50).Nullable()
                .WithColumn("Name").AsString(200).Nullable()
                .WithColumn("LocationId").AsInt32()
                .WithColumn("LocationName").AsString(200).Nullable()
                .WithColumn("LocationAddress").AsString(200).Nullable()
                .WithColumn("StopId").AsInt32()
                .WithColumn("StopOrder").AsInt32()
                .WithColumn("Latitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("Longitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("AdultPrice").AsDecimal(18, 2).NotNullable()
                .WithColumn("ChildrenPrice").AsDecimal(18, 2).NotNullable()
                .WithColumn("FamilyPrice").AsDecimal(18, 2).NotNullable()
                .WithColumn("InfantPrice").AsDecimal(18, 2).NotNullable()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ValueRetailPackageReturns")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ValueRetailPackageRouteId").AsInt32().ForeignKey("ValueRetailPackageRoutes", "Id")
                .WithColumn("RouteId").AsInt32()
                .WithColumn("StopId").AsInt32()
                .WithColumn("StopOrder").AsInt32()
                .WithColumn("Name").AsString(200).Nullable()
                .WithColumn("LocationId").AsInt32()
                .WithColumn("LocationName").AsString(200).Nullable()
                .WithColumn("LocationAddress").AsString(200)
                .WithColumn("ReturnTime").AsString(50).Nullable()
                .WithColumn("Latitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("Longitude").AsDecimal(8, 5).NotNullable()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("GiftCardEventMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventId").AsInt64()
                .WithColumn("GiftCardId").AsInt32()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            //Alter table TRANSACTION DELIVER DETAILS to add columns for courier

            if (!Schema.Table("TransactionMoveAroundMappings").Column("Address1").Exists())
            {
                Alter.Table("TransactionMoveAroundMappings")
                .AddColumn("Address1").AsString(500).Nullable()
                .AddColumn("Address2").AsString(500).Nullable()
                .AddColumn("Town").AsString(100).Nullable()
                .AddColumn("Region").AsString(100).Nullable()
                .AddColumn("PostalCode").AsInt32().Nullable();
            }

            if (!Schema.Table("ValueRetailRoutes").Column("AdultPrice").Exists())
            {
                Alter.Table("ValueRetailRoutes")
                .AddColumn("AdultPrice").AsDecimal(18, 2).NotNullable()
                .AddColumn("ChildrenPrice").AsDecimal(18, 2).NotNullable()
                .AddColumn("FamilyPrice").AsDecimal(18, 2).NotNullable()
                .AddColumn("InfantPrice").AsDecimal(18, 2).NotNullable()
                .AddColumn("UnitPrice").AsDecimal(18, 2).NotNullable();
            }

            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierTown").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierTown")
                .AsString()
                .Nullable();
            }
            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierAddress").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierAddress")
                .AsString()
                .Nullable();
            }
            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierZipcode").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierZipcode")
                .AsInt32()
                .Nullable();
            }
            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierRegion").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierRegion")
                .AsString()
                .Nullable();
            }
            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierCountry").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierCountry")
                .AsString()
                .Nullable();
            }

            Create.Index()
               .OnTable("EventVenueMappings")
               .OnColumn("EventId").Ascending()
               .OnColumn("VenueId").Ascending();

            Create.Index()
               .OnTable("EventVenueMappingTimes")
               .OnColumn("EventVenueMappingId").Ascending();

            Create.Index()
              .OnTable("TransactionMoveAroundMappings")
              .OnColumn("TransactionId").Ascending()
              .OnColumn("EventVenueMappingTimeId").Ascending();
        }
    }
}