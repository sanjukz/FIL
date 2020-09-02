using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20181226160000_BOMissingTables
{
    [Migration(2018, 12, 26, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("VoidRequestDetails").Exists())
            {
                Create.Table("VoidRequestDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("TransactionId ").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("RequestDateTimeUTC").AsDateTime()
                .WithColumn("Reason").AsString(100)
                .WithColumn("IsVoid").AsBoolean()
                .WithColumn("VoidDateTime").AsDateTime()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
                Create.Index()
                   .OnTable("VoidRequestDetails")
                   .OnColumn("Id");
            }

          
        }
    }
}
