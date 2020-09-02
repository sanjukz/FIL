using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20200825120000_DonationDetail
{
    [Migration(2020, 08, 25, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("DonationDetails")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("DonationAmount1").AsDecimal().Nullable()
                 .WithColumn("DonationAmount2").AsDecimal().Nullable()
                 .WithColumn("DonationAmount3").AsDecimal().Nullable()
                 .WithColumn("MinDonation").AsDateTime().Nullable()
                 .WithColumn("MaxDonation").AsDateTime().Nullable()
                 .WithColumn("IsFreeInput").AsBoolean()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}