using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190115190000_EventTicketAttributes
{
    [Migration(2019, 01, 15, 19, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("Eventticketattributes").Column("TicketCategoryNotes").Exists())
            {
                Alter.Table("Eventticketattributes").AddColumn("TicketCategoryNotes").AsString(int.MaxValue).Nullable();
            }
        }
    }
}
