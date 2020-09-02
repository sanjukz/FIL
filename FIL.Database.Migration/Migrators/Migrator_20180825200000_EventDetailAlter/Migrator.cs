using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180825200000_EventDetailAlter
{
    [Migration(2018, 08, 25, 20, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("EventDetails").Column("TicketLimit").Exists())
            {
                Alter.Table("EventDetails")
                .AddColumn("TicketLimit").AsInt32().Nullable();
            }
        }
    }
}
