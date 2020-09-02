using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180727150000_Hubspot
{
    [Migration(2018, 07, 27, 15, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {            
            if (!Schema.Table("HubspotCartTracks").Column("EmailId").Exists())
            {
                Alter.Table("HubspotCartTracks")
                .AddColumn("EmailId")
                .AsString(128).Nullable();                
            }
        }
    }
}
