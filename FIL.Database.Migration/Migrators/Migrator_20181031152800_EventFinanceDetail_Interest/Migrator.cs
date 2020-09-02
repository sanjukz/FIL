using FIL.Database.Migration.Core.Migrators;
using FIL.Database.Migration.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Database.Migration.Migrators.Migrator_20181031152800_EventFinanceDetail_Interest
{
    [Migration(2018, 10, 31, 15, 28, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Down()
        {
            Delete.Table("EventFinanceDetails");
        }

        public override void Up()
        {
            Create.Table("EventFinanceDetails").
                 WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("CurrencyId").AsInt32().NotNullable().ForeignKey("CurrencyTypes", "Id")
                .WithColumn("EventId").AsInt64().NotNullable().ForeignKey("Events", "Id")
                .WithColumn("CountryID").AsInt32().NotNullable().ForeignKey("Countries", "Id")
                .WithColumn("AccountType").AsString().Nullable()
                .WithColumn("StateId").AsInt32().Nullable().ForeignKey("States", "Id")
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().Nullable()
                .WithColumn("BankAccountType").AsString().Nullable()
                .WithColumn("IsBankAccountGST").AsBoolean().Nullable()
                .WithColumn("BankName").AsString().Nullable()
                .WithColumn("RoutingNo").AsString().Nullable()
                .WithColumn("GSTNo").AsString().Nullable()
                .WithColumn("AccountNo").AsString().Nullable()
                .WithColumn("PANNo").AsString().Nullable()
                .WithColumn("AccountNickName").AsString().Nullable()
                .WithColumn("FinancialsAccountBankAccountGSTInfo").AsString().Nullable()
                .WithColumn("IsUpdatLater").AsBoolean().Nullable()
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid().NotNullable()
                .WithColumn("ModifiedBy").AsGuid().Nullable();
        }
    }
}
