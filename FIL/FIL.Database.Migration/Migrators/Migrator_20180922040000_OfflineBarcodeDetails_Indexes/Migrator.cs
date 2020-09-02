using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180922040000_OfflineBarcodeDetails_Indexes
{
  
    [Migration(2018, 09, 22, 04, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("OfflineBarcodeDetails").Index("idx_OfflineBarcodeDetails_BarcodeNumber").Exists())
            {
                Create.Index()
                    .OnTable("OfflineBarcodeDetails")
                    .OnColumn("BarcodeNumber").Ascending();
            }

            if (!Schema.Table("OfflineBarcodeDetails").Index("idx_OfflineBarcodeDetails_EventTicketDetailId").Exists())
            {
                Create.Index()
                    .OnTable("OfflineBarcodeDetails")
                    .OnColumn("EventTicketDetailId").Ascending();
            }
        }
    }
}
