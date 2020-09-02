using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20191212120000_AlgoliaExports
{
    [Migration(2019, 12, 12, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("AlgoliaExports")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("ObjectId").AsInt64()
                  .WithColumn("Name").AsString(500).Nullable()
                  .WithColumn("Description").AsString(int.MaxValue).Nullable()
                  .WithColumn("Category").AsString(500).Nullable()
                  .WithColumn("SubCategory").AsString(500).Nullable()
                  .WithColumn("City").AsString(500).Nullable()
                  .WithColumn("State").AsString(500).Nullable()
                  .WithColumn("Country").AsString(500).Nullable()
                  .WithColumn("Url").AsString(500).Nullable()
                  .WithColumn("PlaceImageUrl").AsString(500).Nullable()
                  .WithColumn("CityId").AsInt64().Nullable()
                  .WithColumn("CountryId").AsInt64().Nullable()
                  .WithColumn("StateId").AsInt64().Nullable()
                  .WithColumn("IsIndexed").AsBoolean()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("AlgoliaCitiesExports")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("ObjectId").AsInt64()
                 .WithColumn("CityName").AsString(500).Nullable()
                 .WithColumn("State").AsString(500).Nullable()
                 .WithColumn("Country").AsString(500).Nullable()
                 .WithColumn("CityId").AsInt64().Nullable()
                 .WithColumn("CountryId").AsInt64().Nullable()
                 .WithColumn("StateId").AsInt64().Nullable()
                 .WithColumn("IsIndexed").AsBoolean()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
