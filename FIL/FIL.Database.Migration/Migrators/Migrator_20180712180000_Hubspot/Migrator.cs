using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180712180000_Hubspot
{
    [Migration(2018, 07, 12, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("HubspotCartTracks")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("HubspotVid").AsInt64()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
