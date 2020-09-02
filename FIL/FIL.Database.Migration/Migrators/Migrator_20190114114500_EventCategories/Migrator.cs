using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190114114500_EventCategories
{
    [Migration(2019, 01, 14, 11, 45, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("EventCategories").Column("DisplayName").Exists())
            {
                Alter.Table("EventCategories").AddColumn("DisplayName").AsString(200).Nullable();
            }

            if (!Schema.Table("EventCategories").Column("Slug").Exists())
            {
                Alter.Table("EventCategories").AddColumn("Slug").AsString(200).Nullable();
            }

            if (!Schema.Table("EventCategories").Column("IsHomePage").Exists())
            {
                Alter.Table("EventCategories").AddColumn("IsHomePage").AsBoolean().Nullable();
            }

            if (!Schema.Table("EventCategories").Column("EventCategoryId").Exists())
            {
                Alter.Table("EventCategories").AddColumn("EventCategoryId").AsInt32().Nullable();
            }

            if (!Schema.Table("EventCategories").Column("IsFeel").Exists())
            {
                Alter.Table("EventCategories").AddColumn("IsFeel").AsBoolean().Nullable();
            }

            if (!Schema.Table("EventCategories").Column("Order").Exists())
            {
                Alter.Table("EventCategories").AddColumn("Order").AsInt32().Nullable();
            }
        }
    }
}
