using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190819220000_ASI
{
    [Migration(2019, 08, 19, 22, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("ASIMonuments")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("MonumentId").AsInt64()
            .WithColumn("Name").AsString()
            .WithColumn("Code").AsString(50)
            .WithColumn("AppConfigVersion").AsString(10).Nullable()
            .WithColumn("Comment").AsString().Nullable()
            .WithColumn("Version").AsString().Nullable()
            .WithColumn("Circle").AsString().Nullable()
            .WithColumn("MaxDate").AsDateTime()
            .WithColumn("Status").AsString()
            .WithColumn("IsOptional").AsBoolean()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASIMonumentEventTableMappings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
            .WithColumn("ASIMonumentId").AsInt64().ForeignKey("ASIMonuments", "Id")
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASITicketTypes")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50)
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASIMonumentDetails")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ASIMonumentId").AsInt64().ForeignKey("ASIMonuments", "Id")
            .WithColumn("Name").AsString()
            .WithColumn("IsOptional").AsBoolean()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASIMonumentTicketTypeMappings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ASIMonumentDetailId").AsInt64().ForeignKey("ASIMonumentDetails", "Id")
            .WithColumn("ASITicketTypeId").AsInt32().ForeignKey("ASITicketTypes", "Id")
            .WithColumn("Total").AsDecimal(18, 2)
            .WithColumn("ASI").AsDecimal(18, 2).Nullable()
            .WithColumn("LDA").AsDecimal(18, 2).Nullable() // Long terms not used bcz it's not getting from API. not used purposely.
            .WithColumn("Others").AsDecimal(18, 2).Nullable()
            .WithColumn("MSM").AsDecimal(18, 2).Nullable()
            .WithColumn("AC").AsDecimal(18, 2).Nullable()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASIMonumentWeekOpenDays")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ASIMonumentId").AsInt64().ForeignKey("ASIMonuments", "Id")
            .WithColumn("DayId").AsInt64().ForeignKey("Days", "Id")
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASIMonumentTimeSlotMappings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ASIMonumentId").AsInt64().ForeignKey("ASIMonuments", "Id")
            .WithColumn("TimeSlotId").AsInt32()
            .WithColumn("Name").AsString()
            .WithColumn("StartTime").AsString()
            .WithColumn("EndTime").AsString()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("EventTimeSlotMappings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
            .WithColumn("TimeSlotId").AsInt32().Nullable()
            .WithColumn("Name").AsString()
            .WithColumn("StartTime").AsString()
            .WithColumn("EndTime").AsString()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASIMonumentHolidayDays")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ASIMonumentId").AsInt64().ForeignKey("ASIMonuments", "Id")
            .WithColumn("HolidayDate").AsDateTime().Nullable()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("CountryRegionalOrganisationMappings")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("CountryId").AsInt32().ForeignKey("Countries", "Id")
            .WithColumn("RegionalOrganisationId").AsInt16().ForeignKey("RegionalOrganisations", "Id")
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            if (!Schema.Table("CustomerDocumentTypes").Column("IsASI").Exists())
            {
                Alter.Table("CustomerDocumentTypes").AddColumn("IsASI").AsBoolean().Nullable();
            }

            Create.Table("GuestDetails")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("TransactionDetailId").AsInt64().ForeignKey("TransactionDetails", "Id")
            .WithColumn("FirstName").AsString()
            .WithColumn("LastName").AsString()
            .WithColumn("Email").AsString()
            .WithColumn("PhoneCode").AsString().Nullable()
            .WithColumn("PhoneNumber").AsString().Nullable()
            .WithColumn("Age").AsString().Nullable()
            .WithColumn("DocumentNumber").AsString().Nullable()
            .WithColumn("CustomerDocumentTypeId").AsInt64().ForeignKey("CustomerDocumentTypes", "Id").Nullable()
            .WithColumn("GenderId").AsInt16().ForeignKey("Genders", "Id").Nullable()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASITransactionDetailTimeSlotIdMappings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("TransactionDetailId").AsInt64().ForeignKey("TransactionDetails", "Id")
            .WithColumn("EventTimeSlotMappingId").AsInt64().ForeignKey("EventTimeSlotMappings", "Id")
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASIPaymentResponseDetails")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Message").AsString().Nullable()
            .WithColumn("IsSuccess").AsBoolean().Nullable()
            .WithColumn("Error").AsString().Nullable()
            .WithColumn("TransactionId").AsString().Nullable()
            .WithColumn("PaymentId").AsString().Nullable()
            .WithColumn("PaymentProvider").AsString().Nullable()
            .WithColumn("PaymentAmount").AsDecimal(18, 2).Nullable()
            .WithColumn("PaymentTransactionId").AsString().Nullable()
            .WithColumn("PaymentGateway").AsString().Nullable()
            .WithColumn("PaymentTimeStamp").AsDateTime().Nullable()
            .WithColumn("PaymentStatus").AsString().Nullable()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ASIPaymentResponseDetailTicketMappings")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ASIPaymentResponseDetailId").AsInt64().ForeignKey("ASIPaymentResponseDetails", "Id")
            .WithColumn("VisitorId").AsString().Nullable()
            .WithColumn("Date").AsDateTime().Nullable()
            .WithColumn("TicketNo").AsString().Nullable()
            .WithColumn("QrCode").AsString().Nullable()
            .WithColumn("Name").AsString().Nullable().Indexed()
            .WithColumn("Gender").AsString().Nullable()
            .WithColumn("Age").AsInt32().Nullable()
            .WithColumn("IdentityType").AsString().Nullable()
            .WithColumn("IdentityNo").AsString().Nullable()
            .WithColumn("NationalityGroup").AsString().Nullable()
            .WithColumn("NationalityCountry").AsString().Nullable()
            .WithColumn("MonumentCode").AsString().Nullable()
            .WithColumn("MonumentName").AsString().Nullable()
            .WithColumn("IsOptional").AsBoolean().Nullable()
            .WithColumn("MonumentTimeSlotId").AsInt32().Nullable()
            .WithColumn("IsAdult").AsBoolean().Nullable()
            .WithColumn("Amount").AsDecimal(18, 2).Nullable()
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Index()
           .OnTable("ASIMonuments")
           .OnColumn("IsEnabled").Ascending();

            Create.Index()
           .OnTable("ASIMonumentEventTableMappings")
           .OnColumn("ASIMonumentId").Ascending()
           .OnColumn("EventId").Ascending()
           .OnColumn("IsEnabled").Ascending();

            Create.Index()
           .OnTable("ASIMonumentDetails")
           .OnColumn("Name").Ascending();

            Create.Index()
           .OnTable("ASIMonumentTicketTypeMappings")
           .OnColumn("ASIMonumentDetailId").Ascending();

            Create.Index()
           .OnTable("EventTimeSlotMappings")
           .OnColumn("EventId").Ascending();

            Create.Index()
           .OnTable("GuestDetails")
           .OnColumn("TransactionDetailId").Ascending();

            Create.Index()
           .OnTable("ASITransactionDetailTimeSlotIdMappings")
           .OnColumn("TransactionDetailId").Ascending()
           .OnColumn("EventTimeSlotMappingId").Ascending();
        }
    }
}