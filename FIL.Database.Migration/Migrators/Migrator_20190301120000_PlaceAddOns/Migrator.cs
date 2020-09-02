using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190301120000_PlaceAddOns
{
    [Migration(2019, 03, 01, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("TicketCategoryTypes")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("Name").AsString(int.MaxValue).Nullable()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TicketCategorySubTypes")
              .WithColumn("Id").AsInt64().PrimaryKey().Identity()
              .WithColumn("TicketCategoryTypeId").AsInt64().ForeignKey("TicketCategoryTypes", "Id")
              .WithColumn("Name").AsString(int.MaxValue).Nullable()
              .WithColumn("IsEnabled").AsBoolean().Indexed()
              .WithColumn("CreatedUtc").AsDateTime()
              .WithColumn("UpdatedUtc").AsDateTime().Nullable()
              .WithColumn("CreatedBy").AsGuid()
              .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventTicketDetailTicketCategoryTypeMappings")
              .WithColumn("Id").AsInt64().PrimaryKey().Identity()
              .WithColumn("EventTicketDetailId").AsInt64().ForeignKey("EventTicketDetails", "Id")
              .WithColumn("TicketCategorySubTypeId").AsInt64().ForeignKey("TicketCategorySubTypes", "Id").Nullable()
              .WithColumn("TicketCategoryTypeId").AsInt64().ForeignKey("TicketCategoryTypes", "Id").Nullable()
              .WithColumn("IsEnabled").AsBoolean().Indexed()
              .WithColumn("CreatedUtc").AsDateTime()
              .WithColumn("UpdatedUtc").AsDateTime().Nullable()
              .WithColumn("CreatedBy").AsGuid()
              .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
