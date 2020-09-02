using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180705150000_ResetPasswordToken
{
    [Migration(2018, 07, 05, 15, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("ResetPasswordTokens")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("TokenId").AsGuid()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();          
        }
    }
}
