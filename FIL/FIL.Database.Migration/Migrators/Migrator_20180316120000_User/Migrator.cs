using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180316120000_UserAccount
{
    [Migration(2018, 03, 16, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_Users_OptOutStatusId_OptOutStatuses_Id")
                .OnTable("Users");

            Delete.ForeignKey("FK_Users_SignUpMethodId_SignUpMethods_Id")
                .OnTable("Users");

            Delete.Column("OptOutStatusId")
                .FromTable("Users");

            Delete.Column("SocialLoginId")
                .FromTable("Users");

            Delete.Column("SignUpMethodId")
                .FromTable("Users");

            //Delete.Column("AddressLine2")
            //    .FromTable("UserAddressDetails");

            //Rename.Column("AddressLine1")
            //    .OnTable("UserAddressDetails")
            //    .To("Address");

            Create.Table("WebsiteUserAdditionalDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("OptedForMailer").AsBoolean().WithDefaultValue(false)
                .WithColumn("OptOutStatusId").AsInt16().ForeignKey("OptOutStatuses", "Id").Nullable()
                .WithColumn("SocialLoginId").AsString(int.MaxValue).Nullable()
                .WithColumn("SignUpMethodId").AsInt16().ForeignKey("SignUpMethods", "Id").Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            if (Schema.Table("EventTicketAttributes").Column("LocalPrice").Exists())
            {
                Delete.Column("LocalPrice")
                .FromTable("EventTicketAttributes");
            }
		}
	}
}
