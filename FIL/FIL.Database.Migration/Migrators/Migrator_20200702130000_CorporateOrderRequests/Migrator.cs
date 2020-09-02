using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrator_20200702130000_CorporateOrderRequests
{
    [Migration(2020, 07, 02, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("CorporateOrderRequests").Exists())
            {
                Create.Table("CorporateOrderRequests")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("SponsorId").AsInt64().ForeignKey("Sponsors", "Id")
                  .WithColumn("OrderTypeId").AsInt16().ForeignKey("OrderTypes", "Id")
                  .WithColumn("AccountTypeId").AsInt16().ForeignKey("AccountTypes", "Id")
                  .WithColumn("EventTicketAttributeId").AsInt64().ForeignKey("EventTicketAttributes", "Id")
                  .WithColumn("RequestedTickets").AsInt16()
                  .WithColumn("AllocatedTickets").AsInt16()
                  .WithColumn("OrderStatusId").AsInt16().ForeignKey("OrderStatuses", "Id")
                  .WithColumn("FirstName").AsString()
                  .WithColumn("LastName").AsString()
                  .WithColumn("Email").AsString()
                  .WithColumn("PhoneCode").AsString()
                  .WithColumn("PhoneNumber").AsString()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
            if (!Schema.Table("CorporateRepresentativeDetails").Exists())
            {
                Create.Table("CorporateRepresentativeDetails")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("CorporateOrderRequestId").AsInt64().ForeignKey("CorporateOrderRequests", "Id")
                  .WithColumn("FirstName").AsString()
                  .WithColumn("LastName").AsString()
                  .WithColumn("Email").AsString()
                  .WithColumn("PhoneCode").AsString()
                  .WithColumn("PhoneNumber").AsString()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
            if (Schema.Table("Sponsors").Exists())
            {
                Rename.Column("CompanyAddress").OnTable("Sponsors").To("Address");
                Rename.Column("CompanyCity").OnTable("Sponsors").To("CityId");
                Rename.Column("CompanyState").OnTable("Sponsors").To("StateId");
                Rename.Column("CompanyCountry").OnTable("Sponsors").To("CountryId");
                Rename.Column("CompanyZipcodeId").OnTable("Sponsors").To("Zipcode");
            }
        }

    }
}
