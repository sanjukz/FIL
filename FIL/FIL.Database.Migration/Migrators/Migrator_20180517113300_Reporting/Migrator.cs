using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180519124500_Reporting
{
    [Migration(2018, 05, 19, 12, 45, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("ReportingUserAdditionalDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("IsCredentialsMailed").AsBoolean()
                .WithColumn("ProfilePic").AsString(200).Nullable()
                .WithColumn("ClientLogo").AsString(200).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedBy").AsGuid().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("Menus")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(200)
                .WithColumn("Url").AsString(200)
                .WithColumn("ParentId").AsInt32()
                .WithColumn("ModuleId").AsInt16().ForeignKey("Modules", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedBy").AsGuid().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("MenusUserMappings")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("MenuId").AsInt32().ForeignKey("Menus", "Id")
               .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
               .WithColumn("SortOrder").AsInt32()
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedBy").AsGuid().Nullable()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("EventsUserMappings")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedBy").AsGuid().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("ReportingColumns")
              .WithColumn("Id").AsInt64().PrimaryKey().Identity()
              .WithColumn("DBFieldName").AsString(500)
              .WithColumn("DisplayName").AsString(500)
              .WithColumn("IsEnabled").AsBoolean()
              .WithColumn("CreatedBy").AsGuid()
              .WithColumn("CreatedUtc").AsDateTime()
              .WithColumn("UpdatedBy").AsGuid().Nullable()
              .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("ReportingColumnsMenuMappings")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("MenuId").AsInt32().ForeignKey("Menus", "Id")
                .WithColumn("ColumnId").AsInt64().ForeignKey("ReportingColumns", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedBy").AsGuid().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("ReportingColumnsUserMappings")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("ColumnsMenuMappingId").AsInt64().ForeignKey("ReportingColumnsMenuMappings", "Id")
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("SortOrder").AsInt32()
                .WithColumn("IsShow").AsBoolean()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedBy").AsGuid().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();
        }
    }
}
