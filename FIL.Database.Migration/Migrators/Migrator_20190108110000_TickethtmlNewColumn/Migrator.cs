using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190108110000_TickethtmlNewColumn
{
    [Migration(2019, 01, 08, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("EventAttributes").Column("MatchAdditionalInfo").Exists())
            {
                Alter.Table("EventAttributes").AddColumn("MatchAdditionalInfo").AsString(2000);
            }
        }
    }
}
