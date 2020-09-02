using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180831160000_TicketAlert
{
    [Migration(2018, 08, 31, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("TicketAlertEventMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id").Nullable()
                .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id").Nullable()
                .WithColumn("CountryId").AsInt32().ForeignKey("Countries", "Id").Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("TicketAlertUserMappings")
               .WithColumn("Id").AsInt32().PrimaryKey().Identity()
               .WithColumn("AltId").AsGuid()
               .WithColumn("TicketAlertEventMappingId").AsInt32().ForeignKey("TicketAlertEventMappings", "Id")
               .WithColumn("FirstName").AsString(100).Nullable()
               .WithColumn("LastName").AsString(100).Nullable()
               .WithColumn("Email").AsString(100)
               .WithColumn("PhoneCode").AsString(10).Nullable()
               .WithColumn("PhoneNumber").AsString(20).Nullable()
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
