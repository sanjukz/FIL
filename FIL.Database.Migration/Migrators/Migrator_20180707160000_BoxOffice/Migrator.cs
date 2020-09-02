using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180707160000_BoxOffice
{
   [Migration(2018, 07, 07, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
           if (!Schema.Table("TicketStockDetails").Exists())
            {
                Create.Table("TicketStockDetails")
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                    .WithColumn("TicketStockStartSrNo").AsString(20)
                    .WithColumn("TicketStockEndSrNo").AsString(20)
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("FloatDetails").Exists())
            {
                Create.Table("FloatDetails")
                   .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                   .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                   .WithColumn("CashInHand").AsDecimal()
                   .WithColumn("CashInHandLocal").AsDecimal()
                   .WithColumn("IsEnabled").AsBoolean()
                   .WithColumn("CreatedUtc").AsDateTime()
                   .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                   .WithColumn("CreatedBy").AsGuid()
                   .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    }
}