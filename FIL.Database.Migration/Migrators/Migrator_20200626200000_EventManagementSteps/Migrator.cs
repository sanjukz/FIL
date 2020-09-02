using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrator_20200626200000_EventManagementSteps
{
    [Migration(2020, 06, 26, 20, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Steps")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Name").AsString()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("StepDetails")
                 .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                 .WithColumn("MasterEventTypeId").AsInt16().ForeignKey("MasterEventTypes", "Id")
                 .WithColumn("StepId").AsInt16().ForeignKey("Steps", "Id")
                 .WithColumn("Description").AsString(500).Nullable()
                 .WithColumn("Icon").AsString(200).Nullable()
                 .WithColumn("Slug").AsString(200).Nullable()
                 .WithColumn("SortOrder").AsInt16()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventStepDetails")
                 .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("CompletedStep").AsString(500)
                 .WithColumn("CurrentStep").AsInt16()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();

            if (!Schema.Table("Events").Column("MasterEventTypeId").Exists())
            {
                Alter.Table("Events")
               .AddColumn("MasterEventTypeId")
               .AsInt16().ForeignKey("MasterEventTypes", "Id")
               .Nullable();
            }

            if (!Schema.Table("Events").Column("EventStatusId").Exists())
            {
                Alter.Table("Events")
               .AddColumn("EventStatusId")
               .AsInt16().ForeignKey("EventStatuses", "Id")
               .Nullable();
            }

            Create.Index()
          .OnTable("MasterEventTypeStepsMappings")
          .OnColumn("MasterEventTypeId").Ascending();
        }
    }
}
