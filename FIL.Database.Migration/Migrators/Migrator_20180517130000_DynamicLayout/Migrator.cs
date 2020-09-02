using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180517130000_DynamicLayout
{
    [Migration(2018, 05, 17, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("DynamicStadiumCoordinate")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("VenueId").AsInt32().ForeignKey("Venues", "Id")
                .WithColumn("Name").AsString(200)
                .WithColumn("DisplayName").AsString(100)
                .WithColumn("SectionCoordinates").AsString(int.MaxValue)
                .WithColumn("SectionTextCoordinates").AsString(int.MaxValue)
                .WithColumn("CircleRectangleValue").AsString(int.MaxValue)
                .WithColumn("Styles").AsString(200)
                .WithColumn("IsDisplay").AsBoolean()
                .WithColumn("IsEnabled").AsBoolean().Indexed()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("DynamicStadiumTicketCategoriesDetails")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("DynamicStadiumCoordinateId").AsInt32().ForeignKey("DynamicStadiumCoordinate", "Id")
                .WithColumn("TicketCategoryId").AsInt32().ForeignKey("TicketCategories", "Id")
                .WithColumn("FillingFastPercentage").AsInt32()
                .WithColumn("IsEnabled").AsBoolean().Indexed()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}