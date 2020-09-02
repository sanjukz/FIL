using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180626130000_EventCategoryMapping
{
    [Migration(2018, 06, 26, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventCategoryMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()                                
                .WithColumn("EventCategoryId").AsInt16().ForeignKey("EventCategories", "Id")
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();            
        }
    }
}
