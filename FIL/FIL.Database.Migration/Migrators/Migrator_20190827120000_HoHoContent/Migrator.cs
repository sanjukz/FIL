using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190827120000_HoHoContent
{
    [Migration(2019, 08, 27, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("CitySightSeeingRoutes")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("EventDetailId").AsInt64()
                  .WithColumn("RouteId").AsString(100).Nullable()
                  .WithColumn("RouteName").AsString(200).Nullable()
                  .WithColumn("RouteColor").AsString(100).Nullable()
                  .WithColumn("RouteDuration").AsString(100).Nullable()
                  .WithColumn("RouteType").AsString(100).Nullable()
                  .WithColumn("RouteStartTime").AsString(100).Nullable()
                  .WithColumn("RouteEndTime").AsString(100).Nullable()
                  .WithColumn("RouteFrequency").AsString(100).Nullable()
                  .WithColumn("RouteAudioLanguages").AsString(100).Nullable()
                  .WithColumn("RouteLiveLanguages").AsString(100).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("CitySightSeeingRouteDetails")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("CitySightSeeingRouteId").AsInt64()
                 .WithColumn("RoutelLocationId").AsString(100).Nullable()
                 .WithColumn("RouteLocationDescription").AsString(300).Nullable()
                 .WithColumn("RouteLocationLatitude").AsString(100).Nullable()
                 .WithColumn("RouteLocationLongitude").AsString(100).Nullable()
                 .WithColumn("RouteLocationStopover").AsBoolean()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
