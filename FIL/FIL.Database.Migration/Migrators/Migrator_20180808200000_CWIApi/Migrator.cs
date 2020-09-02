using FIL.Contracts.Enums;
using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;


namespace FIL.Database.Migration.Migrators.Migrator_20180808200000_CWIApi
{
    [Migration(2018, 08, 08, 20, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
           Create.Table("EventTokenMappings")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("TokenId").AsInt64().ForeignKey("Tokens", "Id").NotNullable()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id").NotNullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
