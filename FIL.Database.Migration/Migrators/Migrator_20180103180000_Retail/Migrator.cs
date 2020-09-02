using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180103180000_Retail
{
    [Migration(2018, 01, 03, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("ChequeDetails")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
               .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
               .WithColumn("BankName").AsString(20).Nullable()
               .WithColumn("ChequeNo").AsString(20).Nullable()
               .WithColumn("ChequeUtc").AsDateTime().Nullable()
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("RefillWalletDetails")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
               .WithColumn("WalletAmount").AsDecimal(18, 2)
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
