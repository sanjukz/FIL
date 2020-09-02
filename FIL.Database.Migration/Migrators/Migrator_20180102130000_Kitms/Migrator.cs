using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180102130000_KITMS
{
    [Migration(2018, 01, 02, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Sponsors")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("SponsorName").AsString(200)
                .WithColumn("FirstName").AsString(20)
                .WithColumn("LastName").AsString(20)
                .WithColumn("Email").AsString(128)
                .WithColumn("PhoneCode").AsString(10)
                .WithColumn("PhoneNumber").AsString(20)
                .WithColumn("Address").AsString(200)
                .WithColumn("ZipcodeId").AsInt32().ForeignKey("Zipcodes", "Id")
                .WithColumn("CompanyAddress").AsString(200)
                .WithColumn("CompanyZipcodeId").AsInt32().ForeignKey("Zipcodes", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
