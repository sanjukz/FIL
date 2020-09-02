using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20181009110000_feelslugMapping
{
    [Migration(2018, 10, 09, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Events").Column("Slug").Exists())
            {
                Alter.Table("Events").AddColumn("Slug").AsString(400).NotNullable();
            }

            if (!Schema.Table("Events").Index("idx_Events_Slug").Exists())
            {
                Create.Index()
                    .OnTable("Events")
                    .OnColumn("Slug");
            }
        }
    }
}
