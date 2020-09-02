using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180724140000_Match
{
    [Migration(2018, 07, 24, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Teams")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("AltId").AsGuid()
               .WithColumn("Name").AsString(100)
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("MatchAttributes")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
               .WithColumn("TeamA").AsInt64().ForeignKey("Teams", "Id")
               .WithColumn("TeamB").AsInt64().ForeignKey("Teams", "Id")
               .WithColumn("MatchNo").AsInt16().Nullable()
               .WithColumn("MatchDay").AsInt16().Nullable()
               .WithColumn("MatchStartTime").AsDateTime()
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
