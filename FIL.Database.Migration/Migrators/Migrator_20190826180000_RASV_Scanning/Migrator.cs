using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190826180000_RASV_Scanning
{

    [Migration(2019, 08, 26, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("RASVVisitorEntryDetails")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("BarcodeNumber").AsString(int.MaxValue)
               .WithColumn("MatchSeatTicketDetailId").AsInt64().Indexed()
               .WithColumn("EntryDateTime").AsDateTime().Indexed()
               .WithColumn("EntryCount").AsInt16().Nullable()
               .WithColumn("OutDateTime").AsDateTime().Nullable()
               .WithColumn("OutCount").AsInt16().Nullable()
               .WithColumn("EntryGate").AsString(int.MaxValue).Nullable()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();


            if (Schema.Table("OfflineBarcodeDetails").Exists())
            {
                Alter.Table("OfflineBarcodeDetails").AddColumn("UniqueId").AsInt32().Nullable();
                Alter.Table("OfflineBarcodeDetails").AddColumn("BarcodeId").AsString(2000).Nullable();
                Alter.Table("OfflineBarcodeDetails").AddColumn("DayAllowed").AsInt32().Nullable();
            }
        }
    }
}