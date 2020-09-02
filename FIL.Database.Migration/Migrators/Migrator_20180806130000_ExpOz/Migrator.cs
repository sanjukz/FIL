using FIL.Contracts.Enums;
using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180806130000_ExpOz
{
    [Migration(2018, 08, 06, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (Schema.Table("Events").Column("IsExpOz").Exists())
            {               
                Delete.Column("IsExpOz")
                .FromTable("Events");
            }

            if (!Schema.Table("Events").Column("EventSource").Exists())
            {
                Alter.Table("Events")
                .AddColumn("EventSourceId")
                .AsInt16().ForeignKey("EventSources", "Id")
                .WithDefaultValue((int)EventSource.None);
            }
        }
    }
}