using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20200815140000_Referrals
{
    [Migration(2020, 08, 15, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Referrals")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("AltId").AsGuid()
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("Name").AsString(int.MaxValue).Nullable()
                 .WithColumn("Code").AsString(500).Nullable()
                 .WithColumn("Description").AsString(int.MaxValue).Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            if (!Schema.Table("Users").Column("ReferralId").Exists())
            {
                Alter.Table("Users").AddColumn("ReferralId").AsInt64().ForeignKey("Referrals", "Id").Nullable();
            }

            if (!Schema.Table("TransactionDetails").Column("ReferralId").Exists())
            {
                Alter.Table("TransactionDetails").AddColumn("ReferralId").AsInt64().ForeignKey("Referrals", "Id").Nullable();
            }

            if (!Schema.Table("DiscountPromoCodes").Column("Limit").Exists())
            {
                Alter.Table("DiscountPromoCodes").AddColumn("Limit").AsInt32().Nullable();
            }
        }
    }
}