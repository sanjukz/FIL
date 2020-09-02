using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180110184600_EventHistory
{
    [Migration(2018, 01, 10, 18, 46, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventHistories")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid().Indexed()
                .WithColumn("EventCategoryId").AsInt16().ForeignKey("EventCategories", "Id")
                .WithColumn("EventTypeId").AsInt16().ForeignKey("EventTypes", "Id")
                .WithColumn("Name").AsString(100)
                .WithColumn("Description").AsString(int.MaxValue).Nullable()
                .WithColumn("ClientPointOfContactId").AsInt32().ForeignKey("ClientPointOfContacts", "Id")
                .WithColumn("FbEventId").AsInt64().Nullable()
                .WithColumn("MetaDetails").AsString(int.MaxValue).Nullable()
                .WithColumn("TermsAndConditions").AsString(int.MaxValue)
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("IsPublishedOnSite").AsBoolean().Nullable()
                .WithColumn("PublishedDateTime").AsDateTime().Nullable()
                .WithColumn("PublishedBy").AsInt32().Nullable()
                .WithColumn("TestedBy").AsInt32().Nullable()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
