using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20191003180000_Tiqets
{
    [Migration(2019, 10, 03, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("TiqetEventDetailMappings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ProductId").AsString()
            .WithColumn("EventDetailId").AsInt64()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TiqetEventTicketDetailMappings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("TiqetVariantDetailId").AsInt64()
            .WithColumn("EventTicketDetailId").AsInt64()
            .WithColumn("ProductId").AsString()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();


            Create.Table("TiqetProductCheckoutDetails")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ProductId").AsString()
            .WithColumn("MustKnow").AsString()
            .WithColumn("GoodToKnow").AsString()
            .WithColumn("PrePurchase").AsString()
            .WithColumn("Usage").AsString()
             .WithColumn("Excluded").AsString()
            .WithColumn("Included").AsString()
            .WithColumn("PostPurchase").AsString()
            .WithColumn("HasTimeSlot").AsBoolean()
            .WithColumn("HasDynamicPrice").AsBoolean()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TiqetProductImages")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ProductId").AsString()
            .WithColumn("Small").AsString()
            .WithColumn("Medium").AsString()
            .WithColumn("Large").AsString()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TiqetProducts")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ProductId").AsString()
            .WithColumn("Tittle").AsString()
            .WithColumn("SaleStatus").AsString()
            .WithColumn("Inclusions").AsString()
            .WithColumn("Language").AsString()
             .WithColumn("CountryName").AsString()
            .WithColumn("CityName").AsString()
            .WithColumn("ProductSlug").AsString()
            .WithColumn("Price").AsDecimal(18, 2)
            .WithColumn("SaleStatuReason").AsString()
            .WithColumn("VenueName").AsString()
            .WithColumn("VenueAddress").AsString()
            .WithColumn("Summary").AsString()
            .WithColumn("TagLine").AsString()
            .WithColumn("PromoLabel").AsString()
            .WithColumn("RatingAverage").AsString()
            .WithColumn("GeoLocationLatitude").AsString()
            .WithColumn("GeoLocationLongitude").AsString()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TiqetProductTagMappings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ProductId").AsString()
            .WithColumn("TagId").AsString()
            .WithColumn("IsCategoryType").AsBoolean()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TiqetTags")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString()
            .WithColumn("TagId").AsString()
            .WithColumn("TagTypeId").AsString()
            .WithColumn("TagTypeId").AsInt64()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TiqetTagTypes")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Name").AsString()
            .WithColumn("TypeId").AsString()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TiqetVariantDetails")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ProductId").AsString()
            .WithColumn("VariantId").AsString()
                        .WithColumn("Label").AsString()
            .WithColumn("MaxTicketsPerOrder").AsInt32()
            .WithColumn("MaxTicketsPerOrder").AsInt32()
            .WithColumn("DistributorCommissionExclVat").AsDecimal(18, 2)
            .WithColumn("TotalRetailPriceInclVat").AsDecimal(18, 2)
            .WithColumn("SaleTicketValueInclVat").AsDecimal(18, 2)
            .WithColumn("BookingFeeInclVat").AsDecimal(18, 2)
            .WithColumn("DynamicVariantPricing").AsBoolean()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Index()
           .OnTable("TiqetVariantDetails")
           .OnColumn("ProductId").Ascending();

            Create.Index()
          .OnTable("TiqetProductTagMappings")
          .OnColumn("ProductId").Ascending();

            Create.Index()
          .OnTable("TiqetProductImages")
          .OnColumn("ProductId").Ascending();

            Create.Index()
          .OnTable("TiqetProducts")
          .OnColumn("ProductId").Ascending();

            Create.Index()
          .OnTable("TiqetProductCheckoutDetails")
          .OnColumn("ProductId").Ascending();

            Create.Index()
          .OnTable("TiqetEventTicketDetailMappings")
          .OnColumn("ProductId").Ascending(); 
        }
    }
}