using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190111190000_EventTicketAttributes
{
    [Migration(2019, 01, 11, 19, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("EventTicketAttributes").Column("TicketValidityType").Exists())
            {
                Alter.Table("EventTicketAttributes").AddColumn("TicketValidityType").AsInt16().ForeignKey("TicketValidityTypes", "Id").Nullable();
            }

            if (!Schema.Table("EventTicketAttributes").Column("TicketValidity").Exists())
            {
                Alter.Table("EventTicketAttributes").AddColumn("TicketValidity").AsString(int.MaxValue).Nullable();
            }
        }
    }
}
