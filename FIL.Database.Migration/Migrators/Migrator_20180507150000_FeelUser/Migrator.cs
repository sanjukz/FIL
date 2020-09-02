using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180507150000_FeelUser
{
    [Migration(2018, 05, 07, 15, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("FeelUserAdditionalDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("OptedForMailer").AsBoolean().WithDefaultValue(false)
                .WithColumn("OptOutStatusId").AsInt16().ForeignKey("OptOutStatuses", "Id").Nullable()
                .WithColumn("SocialLoginId").AsString(int.MaxValue).Nullable()
                .WithColumn("SignUpMethodId").AsInt16().ForeignKey("SignUpMethods", "Id").Nullable()
                .WithColumn("BirthDate").AsDate()
                .WithColumn("Gender").AsInt16().ForeignKey("Genders", "Id")
                .WithColumn("ResidentId").AsInt32().ForeignKey("Countries", "Id")
                .WithColumn("CitizenId").AsInt32().ForeignKey("Countries", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
