using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180615140000_EventLearnMoreAttributes
{
    [Migration(2018, 06, 15, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventLearnMoreAttributes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("LearnMoreFeatureId").AsInt16().ForeignKey("LearnMoreFeatures", "Id")
                .WithColumn("Description").AsString(int.MaxValue).Nullable()
                .WithColumn("Image").AsString(100).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Index()
                .OnTable("EventLearnMoreAttributes")
                .OnColumn("AltId");
        }
    }
}
