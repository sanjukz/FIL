using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20171002100000_Discount
{
    [Migration(2017, 10, 02, 10, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Discounts")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(20)
                .WithColumn("DiscountTypeId").AsInt16().ForeignKey("DiscountTypes", "Id")
                .WithColumn("Description").AsString(200).Nullable()
                .WithColumn("DiscountValueTypeId").AsInt16().ForeignKey("DiscountValueTypes", "Id")
                .WithColumn("DiscountValue").AsDecimal()
                .WithColumn("MaximumDiscountPerTransaction").AsDecimal().Nullable()
                .WithColumn("MinimumQuantityPerTransaction").AsInt16().Nullable()
                .WithColumn("MaximumQuantityPerTransaction").AsInt16().Nullable()
                .WithColumn("OverallMaximumDiscount").AsDecimal().Nullable()
                .WithColumn("OverallMaximumQuantity").AsInt16().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
