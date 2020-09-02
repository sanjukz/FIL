using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180904210000_MasterDynamicLayout
{
    [Migration(2018, 09, 04, 21, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("MasterDynamicStadiumCoordinates")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("MasterVenueLayoutId").AsInt32().ForeignKey("MasterVenueLayouts", "Id")
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

            Create.Table("MasterDynamicStadiumSectionDetails")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("MasterDynamicStadiumCoordinateId").AsInt32().ForeignKey("MasterDynamicStadiumCoordinates", "Id")
                .WithColumn("MasterVenueLayoutSectionId").AsInt32().ForeignKey("MasterVenueLayoutSections", "Id")
                .WithColumn("IsEnabled").AsBoolean().Indexed()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
