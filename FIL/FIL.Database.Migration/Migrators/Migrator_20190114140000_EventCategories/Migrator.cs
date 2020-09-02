using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190114150000_EventCategories
{
    [Migration(2019, 01, 14, 15, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (Schema.Table("EventCategories").Column("EventCategory").Exists())
            {
                Rename.Column("EventCategory")
                    .OnTable("EventCategories")
                    .To("Category");
            }
        }
    }
}
