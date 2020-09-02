using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migrator_20190725150000_ValueRetailEventVenueMappingTimesColumnAlter
{
    [Migration(2019, 07, 25, 15, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("EventVenueMappingTimes").Column("WaitingTime").Exists())
            {
                Alter.Table("EventVenueMappingTimes").AddColumn("WaitingTime").AsString(100).Nullable(); 
            }
        }
    }
}