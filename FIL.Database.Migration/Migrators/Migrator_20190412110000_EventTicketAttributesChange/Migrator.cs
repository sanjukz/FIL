using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190412110000_EventTicketAttributesChange
{
    [Migration(2019, 04, 12, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Eventticketattributes").Column("SpecialPrice").Exists())
            {
                Alter.Table("Eventticketattributes").AddColumn("SpecialPrice").AsDecimal(18, 2).Nullable();
            }
        }
    }
}
