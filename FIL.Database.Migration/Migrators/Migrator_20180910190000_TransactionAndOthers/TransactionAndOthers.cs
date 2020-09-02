using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Database.Migration.Migrators.Migrator_20180910190000_TransactionAndOthers
{
    [Migration(2018, 09, 10, 19, 0, 0)]
    public class TransactionAndOthers : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("TransactionBarcodeReversalLogs").Exists())
            {
                Create.Table("TransactionBarcodeReversalLogs")
                     .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                     .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                     .WithColumn("BarcodeNumber").AsString(int.MaxValue).Nullable()
                     .WithColumn("IsEnabled").AsBoolean()
                     .WithColumn("CreatedUtc").AsDateTime()
                     .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                     .WithColumn("CreatedBy").AsGuid()
                     .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("TransactionReleasedLog").Exists())
            {
                Create.Table("TransactionReleasedLog")
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id")
                    .WithColumn("ReleasedDateTime").AsDateTime()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("ValueAddedTaxDetails").Exists())
            {
                Create.Table("ValueAddedTaxDetails")
                     .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                     .WithColumn("RegistrationNumber").AsString(int.MaxValue).Nullable()
                     .WithColumn("Value").AsDecimal().Nullable()
                     .WithColumn("ValueTypeId").AsInt16().ForeignKey("ValueTypes", "Id").Nullable()
                     .WithColumn("SponsorId").AsInt64().ForeignKey("Sponsors", "Id").Nullable()
                     .WithColumn("VenueId").AsInt32().ForeignKey("Venues", "Id").NotNullable()
                     .WithColumn("CompanyName").AsString(int.MaxValue).Nullable()
                     .WithColumn("CompanyAddress").AsString(int.MaxValue).Nullable()
                     .WithColumn("CompanyLogo").AsString(int.MaxValue).Nullable()
                     .WithColumn("IsEnabled").AsBoolean()
                     .WithColumn("CreatedUtc").AsDateTime()
                     .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                     .WithColumn("CreatedBy").AsGuid()
                     .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("VisitorDetails").Exists())
            {
                Create.Table("VisitorDetails")
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id").NotNullable()
                    .WithColumn("Name").AsString(400).Nullable()
                    .WithColumn("PhoneCode").AsString(200).Nullable()
                    .WithColumn("MobileNumber").AsString(400).Nullable()
                    .WithColumn("Email").AsString(400).Nullable()
                    .WithColumn("IdType").AsString(200).Nullable()
                    .WithColumn("IdNumber").AsString(200).Nullable()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("WheelchairSeatMappings").Exists())
            {
                Create.Table("WheelchairSeatMappings")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("MatchSeatTicketDetailId").AsInt64().ForeignKey("MatchSeatTicketDetails", "Id").Nullable()
                    .WithColumn("IsWheelChair").AsInt32().Nullable()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("RePrintRequestDetails").Exists())
            {
                Create.Table("RePrintRequestDetails")
                   .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                   .WithColumn("RePrintRequestId").AsInt64().ForeignKey("ReprintRequests", "Id")
                   .WithColumn("MatchSeatTicketDetailId").AsInt64().ForeignKey("MatchSeatTicketDetails", "Id").Nullable()
                   .WithColumn("BarcodeNumber").AsString(200).Nullable()
                   .WithColumn("IsRePrinted").AsBoolean()
                   .WithColumn("RePrintCount").AsInt32().Nullable()
                   .WithColumn("CreatedUtc").AsDateTime()
                   .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                   .WithColumn("CreatedBy").AsGuid()
                   .WithColumn("UpdatedBy").AsGuid().Nullable()
                   .WithColumn("IsApproved").AsInt32().Nullable()
                   .WithColumn("ApprovedDateTime").AsDateTime().Nullable();             
            }

            if (!Schema.Table("ResetPasswordTokens").Exists())
            {
                Create.Table("ResetPasswordTokens")
                   .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                   .WithColumn("TokenId").AsGuid()
                   .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id").Nullable()
                   .WithColumn("IsEnabled").AsBoolean()
                   .WithColumn("CreatedUtc").AsDateTime()
                   .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                   .WithColumn("CreatedBy").AsGuid()
                   .WithColumn("UpdatedBy").AsGuid().Nullable()
                   .WithColumn("IsApproved").AsInt32().Nullable()
                   .WithColumn("ApprovedDateTime").AsDateTime().Nullable();
            }
        }
    }
}
