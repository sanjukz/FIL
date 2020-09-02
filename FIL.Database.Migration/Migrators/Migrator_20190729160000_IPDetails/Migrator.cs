using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190729160000_IPDetails
{
    [Migration(2019, 07, 29, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("IPDetails").Column("IPDetails").Exists())
            {
                Alter.Column("IPAddress")
               .OnTable("IPDetails")
               .AsString(500)
               .NotNullable();

                Alter.Column("City")
               .OnTable("IPDetails")
               .AsString(500)
               .Nullable();

                Alter.Column("CountryName")
               .OnTable("IPDetails")
               .AsString(500)
               .Nullable();

                Alter.Column("TimeZone")
               .OnTable("IPDetails")
               .AsString(500)
               .Nullable();

                Alter.Column("CountryCode")
               .OnTable("IPDetails")
               .AsString(200)
               .Nullable();

                Alter.Column("Zipcode")
               .OnTable("IPDetails")
               .AsString(100)
               .Nullable();

                Alter.Column("RegionCode")
                .OnTable("IPDetails")
                .AsString(200)
                .Nullable();

                Alter.Column("RegionName")
                .OnTable("IPDetails")
                .AsString(500)
                .Nullable();
            }
        }
    }
}
