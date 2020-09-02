using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190916180000_FeelDynamicLayout
{
    [Migration(2019, 09, 16, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("PageViews")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PageName").AsString()
                .WithColumn("IsEnabled").AsBoolean();

            Create.Table("SectionViews")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PageViewId").AsInt32().ForeignKey("PageViews", "Id")
                .WithColumn("SectionName").AsString()
                .WithColumn("ComponentName").AsString().Nullable()
                .WithColumn("SectionHeading").AsString().Nullable()
                .WithColumn("SectionSubHeading").AsString().Nullable()
                .WithColumn("Endpoint").AsString().Nullable()
                .WithColumn("SortOrder").AsInt32()
                .WithColumn("IsEnabled").AsBoolean();
        }
    }
}
