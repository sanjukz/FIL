using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180501400000_Event
{
    [Migration(2018, 05, 01, 40, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("Ratings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("Points").AsInt16()
                .WithColumn("Comment").AsString(int.MaxValue).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("FormFields")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Caption").AsString(500)
                .WithColumn("Type").AsString(100).Nullable()
                .WithColumn("ValidationScheme").AsString(100).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventFormFields")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("FormFieldId").AsInt32().ForeignKey("FormFields", "Id")
                .WithColumn("Value").AsString(int.MaxValue).Nullable()
                .WithColumn("IsMandatory").AsBoolean().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventAttendeeDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("TransactionDetailId").AsInt64().ForeignKey("TransactionDetails", "Id").Nullable()
                .WithColumn("EventFormFieldId").AsInt64().ForeignKey("EventFormFields", "Id")
                .WithColumn("Value").AsString(int.MaxValue)
                .WithColumn("AttendeeNumber").AsInt16()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventAmenities")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                .WithColumn("AmenityId").AsInt16().ForeignKey("Amenities", "Id")
                .WithColumn("Description").AsString(int.MaxValue)
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
