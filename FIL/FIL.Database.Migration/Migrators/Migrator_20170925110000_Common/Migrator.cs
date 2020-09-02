using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20170925110000_Common
{
    [Migration(2017, 09, 25, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Countries")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("Name").AsString(100)
                .WithColumn("IsoAlphaTwoCode").AsString(20)
                .WithColumn("IsoAlphaThreeCode").AsString(20)
                .WithColumn("Numcode").AsInt32().Nullable()
                .WithColumn("Phonecode").AsInt32().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("States")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("Name").AsString(100)
                .WithColumn("Abbreviation").AsString(20).Nullable()
                .WithColumn("CountryId").AsInt32().ForeignKey("Countries", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Cities")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("Name").AsString(100)
                .WithColumn("StateId").AsInt32().ForeignKey("States", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Zipcodes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("Postalcode").AsString(20)
                .WithColumn("Region").AsString(100).Nullable()
                .WithColumn("CityId").AsInt32().ForeignKey("Cities", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Venues")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("Name").AsString(500)
                .WithColumn("AddressLineOne").AsString(500)
                .WithColumn("AddressLineTwo").AsString(500).Nullable()
                .WithColumn("CityId").AsInt32().ForeignKey("Cities", "Id")
                .WithColumn("Latitude").AsString().Nullable()
                .WithColumn("Longitude").AsString().Nullable()
                .WithColumn("HasImages").AsBoolean().Nullable()
                .WithColumn("Prefix").AsString(20).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("CurrencyTypes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Code").AsString(10)
                .WithColumn("Name").AsString(20)
                .WithColumn("CountryId").AsInt32().ForeignKey("Countries", "Id")
                .WithColumn("ExchangeRate").AsDecimal(18, 2).Nullable()
                .WithColumn("Taxes").AsDecimal(18, 2).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Activities")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Module").AsString(100)
                .WithColumn("TableName").AsString(100)
                .WithColumn("Description").AsString(int.MaxValue).Nullable()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid();
        }
    }
}
