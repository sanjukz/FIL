using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180221130000_Common
{
    [Migration(2018, 02, 21, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("VoidRequests")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("RequestDateTime").AsDateTime()
                .WithColumn("Remarks").AsString(200)
                .WithColumn("IsVoided").AsBoolean()
                .WithColumn("VoidedBy").AsInt64().Nullable()
                .WithColumn("VoidedDateTime").AsDateTime()
                .WithColumn("ModuleId").AsInt16().ForeignKey("Modules", "Id")
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ReprintRequests")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("RequestDateTime").AsDateTime()
                .WithColumn("Remarks").AsString(200)
                .WithColumn("IsApproved").AsBoolean()
                .WithColumn("ApprovedBy").AsInt64().ForeignKey("Users", "Id").Nullable()
                .WithColumn("ApprovedDateTime").AsDateTime()
                .WithColumn("ModuleId").AsInt16().ForeignKey("Modules", "Id")
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("OfflineCustomers")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("PaymentTypeId").AsInt16().ForeignKey("PaymentTypes", "Id").Nullable()
                .WithColumn("IDType").AsString().Nullable()
                .WithColumn("IDNumber").AsString().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("ModuleId").AsInt16().ForeignKey("Modules", "Id")
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
