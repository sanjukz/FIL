using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190227120000_Amenities
{
    [Migration(2019, 02, 27, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Amenities").Exists())
            {
                Create.Table("Amenities")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Amenity").AsString(int.MaxValue);
            }
            if (!Schema.Table("Users").Column("IsRASVMailOPT").Exists())
            {
                Alter.Table("Users").AddColumn("IsRASVMailOPT").AsBoolean().Nullable();
            }
        }
    }
}
