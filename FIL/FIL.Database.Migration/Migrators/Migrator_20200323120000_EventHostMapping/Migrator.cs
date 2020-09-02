using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrator_20200323120000_EventHostMapping
{
    [Migration(2020, 03, 23, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {

            Create.Table("EventHostMappings")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("AltId").AsGuid()
                  .WithColumn("FirstName").AsString(100)
                  .WithColumn("LastName").AsString(100).Nullable()
                  .WithColumn("Email").AsString(200).Nullable()
                  .WithColumn("Description").AsString(int.MaxValue).Nullable()
                  .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();


            // Creating indexes --------------------------
            Create.Index().OnTable("EventHostMappings")
                .OnColumn("AltId").Ascending();

            Create.Index().OnTable("EventHostMappings")
                .OnColumn("EventId").Ascending();
        }
    }
}
