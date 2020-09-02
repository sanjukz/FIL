using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180719160000_RatingTableAlter
{
    [Migration(2018, 07, 19, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Ratings").Column("AltId").Exists())
            {
                Alter.Table("Ratings")
                .AddColumn("AltId").AsGuid().Nullable();
            }
        }
    }
}
