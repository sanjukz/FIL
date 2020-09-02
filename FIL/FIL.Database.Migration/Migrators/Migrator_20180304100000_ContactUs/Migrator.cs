using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180304100000_ContactUs
{
    [Migration(2018, 03, 04, 10, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("ContactUsDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()                
                .WithColumn("FirstName").AsString(20)
                .WithColumn("LastName").AsString(20)
                .WithColumn("Email").AsString(128)
                .WithColumn("PhoneCode").AsString(10)
                .WithColumn("PhoneNumber").AsString(20)
                .WithColumn("Subject").AsString(int.MaxValue)
                .WithColumn("Status").AsString(200)                
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedBy").AsGuid().Nullable();               
        }
    }
}
