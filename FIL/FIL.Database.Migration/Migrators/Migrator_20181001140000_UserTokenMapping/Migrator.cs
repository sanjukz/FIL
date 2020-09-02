using FIL.Contracts.Enums;
using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20181001140000_UserTokenMapping
{
    [Migration(2018, 10, 01, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("UserTokenMappings").Exists())
            {
                Create.Table("UserTokenMappings")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("TokenId").AsInt64().ForeignKey("Tokens", "Id").NotNullable()
                 .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id").NotNullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    }
}
