using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190417110000_EventTicketAttributesChanges
{
    [Migration(2019, 04, 17, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Eventticketattributes").Column("SpecialSeasonPrice").Exists())
            {
                Alter.Table("Eventticketattributes").AddColumn("SpecialSeasonPrice").AsDecimal(18, 2).Nullable();
            }
        }
    }
}
