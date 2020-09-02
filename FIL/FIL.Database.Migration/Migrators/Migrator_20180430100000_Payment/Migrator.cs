using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180430100000_Payment
{
    [Migration(2018, 04, 30, 10, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("NetBankingBankDetails")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("BankName").AsString(200).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("CashCardDetails")
              .WithColumn("Id").AsInt32().PrimaryKey().Identity()
              .WithColumn("AltId").AsGuid()
              .WithColumn("CardName").AsString(200).Nullable()
              .WithColumn("IsEnabled").AsBoolean()
              .WithColumn("CreatedUtc").AsDateTime()
              .WithColumn("UpdatedUtc").AsDateTime().Nullable()
              .WithColumn("CreatedBy").AsGuid()
              .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}