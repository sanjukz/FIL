using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180523163000_Kitms
{
    [Migration(2018, 05, 23, 16, 30, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("UserVenueMappings")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
            .WithColumn("VenueId").AsInt32().ForeignKey("Venues", "Id")
            .WithColumn("IsEnabled").AsBoolean()
            .WithColumn("CreatedBy").AsGuid()
            .WithColumn("CreatedUtc").AsDateTime()
            .WithColumn("UpdatedBy").AsGuid().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            Create.Table("EventSponsorMappings")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("SponsorId").AsInt64().ForeignKey("Sponsors", "Id")
                .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedBy").AsGuid().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();

            if (Schema.Table("UserVenueMappings").Exists())
            {
                Delete.Table("SponsorBlockedTicketDetailActions");
            }
            if (Schema.Table("UserVenueMappings").Exists())
            {
                Delete.Table("SponsorBlockedTicketDetails");
            }
            
            Create.Table("CorporateTicketAllocationDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("EventTicketAttributeId").AsInt64().ForeignKey("EventTicketAttributes", "Id")
                .WithColumn("SponsorId").AsInt64().ForeignKey("Sponsors", "Id")
                .WithColumn("AllocatedTickets").AsInt16().Nullable()
                .WithColumn("RemainingTickets").AsInt16().Nullable()
                .WithColumn("Price").AsDecimal(18, 2)
                .WithColumn("IsCorporateRequest").AsBoolean().Nullable()
                .WithColumn("CorporateRequestId").AsInt64().ForeignKey("CorporateRequests", "Id").Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("CorporateTicketAllocationDetailLogs")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("CorporateTicketAllocationDetailId").AsInt64().ForeignKey("CorporateTicketAllocationDetails", "Id")
               .WithColumn("TransferToCorporateTicketAllocationDetailId").AsInt64().ForeignKey("CorporateTicketAllocationDetails", "Id").Nullable()
               .WithColumn("AllocationOptionId").AsInt16().ForeignKey("AllocationOptions", "Id")
               .WithColumn("TotalTickets").AsInt16()
               .WithColumn("Price").AsDecimal(18, 2)
               .WithColumn("IsEnabled").AsBoolean()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("UpdatedBy").AsGuid().Nullable();

            if (!Schema.Table("Sponsors").Column("SponsorTypeId").Exists())
            {
                Alter.Table("Sponsors")
                .AddColumn("SponsorTypeId")
                .AsInt16()
                .ForeignKey("SponsorTypes", "Id");
            }

            if (Schema.Table("UserVenueMappings").Exists())
            {
                Delete.Table("SponsorTransactionDetails");
            }
            if (Schema.Table("UserVenueMappings").Exists())
            {
                Delete.Table("SponsorPaidTransactionPaymentDetails");
            }

            Create.Table("CorporateTransactionDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("EventTicketAttributeId").AsInt64().ForeignKey("EventTicketAttributes", "Id")
                .WithColumn("SponsorId").AsInt64().ForeignKey("Sponsors", "Id")
                .WithColumn("TotalTickets").AsInt16()
                .WithColumn("Price").AsDecimal(18, 2)
                .WithColumn("TransactingOptionId").AsInt16().ForeignKey("TransactingOptions", "Id")
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("CorporateTransactionPaymentDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("AltId").AsGuid()
                .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                .WithColumn("TransactingOptionId").AsInt16().ForeignKey("TransactingOptions", "Id")
                .WithColumn("PaymentTypeId").AsInt16().ForeignKey("PaymentTypes", "Id")
                .WithColumn("PaymentReceivedIn").AsString(20).Nullable()
                .WithColumn("PaymentConfirmedBy").AsString(40).Nullable()
                .WithColumn("PaymentApprovedUtc").AsDateTime().Nullable()
                .WithColumn("ChequeDrawnonBank").AsString(40).Nullable()
                .WithColumn("ChequeUtc").AsDateTime().Nullable()
                .WithColumn("ApprovalEmailFrom").AsString(128).Nullable()
                .WithColumn("ChequeNumber").AsString(40).Nullable()
                .WithColumn("ApprovalEmailReceivedOn").AsDateTime().Nullable()
                .WithColumn("BankReferenceNumber").AsString(40).Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            if (!Schema.Table("MatchSeatTicketDetails").Column("SponsorId").Exists())
            {
                Alter.Table("MatchSeatTicketDetails")
                .AddColumn("SponsorId")
                .AsInt64()
                .ForeignKey("Sponsors", "Id").Nullable();
            }

            if (!Schema.Table("MatchSeatTicketDetails").Column("TransactionId").Exists())
            {
                Alter.Table("MatchSeatTicketDetails")
                .AddColumn("TransactionId")
                .AsInt64()
                .ForeignKey("Transactions", "Id").Nullable();
            }

            if (!Schema.Table("EventTicketAttributes").Column("LocalCurrencyId").Exists())
            {
                Alter.Table("EventTicketAttributes")
                .AddColumn("LocalCurrencyId")
                .AsInt32()
                .ForeignKey("CurrencyTypes", "Id").Nullable();
            }

            if (!Schema.Table("EventTicketAttributes").Column("LocalPrice").Exists())
            {
                Alter.Table("EventTicketAttributes")
                .AddColumn("LocalPrice")
                .AsDecimal(18, 2).Nullable();
            }
        }
    }
}
