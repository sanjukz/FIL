using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180801190000_OfflineBarcode
{
    [Migration(2018, 08, 01, 19, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("OfflineBarcodeDetails")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("AltId").AsGuid()
                  .WithColumn("EventTicketDetailId").AsInt64().ForeignKey("EventTicketDetails", "Id")
                  .WithColumn("Price").AsDecimal()
                  .WithColumn("CurrencyId").AsInt32().ForeignKey("CurrencyTypes", "Id")
                  .WithColumn("BarcodeNumber").AsString(50).Nullable()
                  .WithColumn("AdditionalInfo").AsString(int.MaxValue)
                  .WithColumn("EntryCount").AsInt32().Nullable()
                  .WithColumn("EntryStatus").AsBoolean().Nullable()
                  .WithColumn("EntryDateTime").AsDateTime().Nullable()
                  .WithColumn("CheckedDateTime").AsDateTime().Nullable()
                  .WithColumn("EntryCountAllowed").AsInt32().Nullable()
                  .WithColumn("IsConsumed").AsInt32().Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}