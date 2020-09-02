using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180312160000_PlaceVisitDuration
{

    [Migration(2018, 03, 12, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("PlaceVisitDurations")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("AltId").AsGuid().Nullable()
               .WithColumn("EventId").AsInt64()
               .WithColumn("TimeDuration").AsString().Nullable()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
