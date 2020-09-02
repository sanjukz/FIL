using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20171016180000_UserAccount
{
    [Migration(2017, 10, 16, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid().Indexed()
                .WithColumn("RolesId").AsInt32().ForeignKey("Roles", "Id").Nullable()
                .WithColumn("UserName").AsString(200)
                .WithColumn("Password").AsString(int.MaxValue)
                .WithColumn("FirstName").AsString(20)
                .WithColumn("LastName").AsString(20)
                .WithColumn("Email").AsString(128).Nullable()
                .WithColumn("EmailVerified").AsBoolean().Nullable()
                .WithColumn("PhoneCode").AsString(10).Nullable()
                .WithColumn("PhoneNumber").AsString(20).Nullable()
                .WithColumn("PhoneConfirmed").AsBoolean().WithDefaultValue(false)
                .WithColumn("OptOutStatusId").AsInt16().ForeignKey("OptOutStatuses", "Id").Nullable()
                .WithColumn("SocialLoginId").AsString(int.MaxValue).Nullable()
                .WithColumn("SignUpMethodId").AsInt16().ForeignKey("SignUpMethods", "Id").Nullable()
                .WithColumn("ChannelId").AsInt16().ForeignKey("Channels", "Id").Nullable()
                .WithColumn("IsActivated").AsBoolean().Nullable()
                .WithColumn("LockOutEndDateUtc").AsDateTime().Nullable()
                .WithColumn("LockOutEnabled").AsBoolean().WithDefaultValue(false)
                .WithColumn("AccessFailedCount").AsInt16().WithDefaultValue(0)
                .WithColumn("LoginCount").AsInt32().WithDefaultValue(0)
                .WithColumn("SecurityStamp").AsInt32().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("UserAddressDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("AltId").AsGuid()
                .WithColumn("FirstName").AsString(20)
                .WithColumn("LastName").AsString(20)
                .WithColumn("PhoneCode").AsString(10)
                .WithColumn("PhoneNumber").AsString(20)
                .WithColumn("AddressLine1").AsString(200)
                .WithColumn("AddressLine2").AsString(200).Nullable()
                .WithColumn("Zipcode").AsInt32().ForeignKey("Zipcodes", "Id")
                .WithColumn("AddressTypeId").AsInt16().ForeignKey("AddressTypes", "Id").Nullable()
                .WithColumn("IsDefault").AsBoolean().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("UserCardDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("AltId").AsGuid()
                .WithColumn("NameOnCard").AsString(50)
                .WithColumn("CardNumber").AsString(int.MaxValue)
                .WithColumn("ExpiryMonth").AsInt16().Nullable()
                .WithColumn("ExpiryYear").AsInt16().Nullable()
                .WithColumn("CardTypeId").AsInt16().ForeignKey("CardTypes", "Id").Nullable()
                .WithColumn("IsDefault").AsBoolean().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
