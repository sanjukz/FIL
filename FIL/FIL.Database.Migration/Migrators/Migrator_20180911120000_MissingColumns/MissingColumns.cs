using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Database.Migration.Migrators.Migrator_20180911120000_MissingColumns
{
    [Migration(2018, 09, 11, 12, 0, 0)]
    public class MissingColumns : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("CorporateRequests").Column("City").Exists())
            {
                Alter.Table("CorporateRequests").AddColumn("City").AsString(400).Nullable();
            }
            if (!Schema.Table("CorporateRequests").Column("State").Exists())
            {
                Alter.Table("CorporateRequests").AddColumn("State").AsString(400).Nullable();
            }
            if (!Schema.Table("CorporateRequests").Column("Country").Exists())
            {
                Alter.Table("CorporateRequests").AddColumn("Country").AsString(400).Nullable();
            }
            if (!Schema.Table("CorporateRequests").Column("ZipCode").Exists())
            {
                Alter.Table("CorporateRequests").AddColumn("ZipCode").AsString(400).Nullable();
            }
            if (!Schema.Table("CorporateRequests").Column("Classification").Exists())
            {
                Alter.Table("CorporateRequests").AddColumn("Classification").AsString(1000).Nullable();
            }
            if (!Schema.Table("UserAddressDetails").Column("Address").Exists())
            {
                Alter.Table("UserAddressDetails").AddColumn("Address").AsString(400).Nullable();
            }
            if (!Schema.Table("TransactionDetails").Column("IsSeasonPackage").Exists())
            {
                Alter.Table("TransactionDetails").AddColumn("IsSeasonPackage").AsInt32().Nullable();
            }
            if (!Schema.Table("TournamentLayoutSections").Column("SectionId").Exists())
            {
                Alter.Table("TournamentLayoutSections").AddColumn("SectionId").AsInt32().Nullable();
            }
            if (!Schema.Table("Sponsors").Column("CompanyCity").Exists())
            {
                Alter.Table("Sponsors").AddColumn("CompanyCity").AsString(400).Nullable();
            }
            if (!Schema.Table("Sponsors").Column("CompanyState").Exists())
            {
                Alter.Table("Sponsors").AddColumn("CompanyState").AsString(400).Nullable();
            }
            if (!Schema.Table("Sponsors").Column("CompanyCountry").Exists())
            {
                Alter.Table("Sponsors").AddColumn("CompanyCountry").AsString(400).Nullable();
            }
            if (!Schema.Table("Sponsors").Column("IdType").Exists())
            {
                Alter.Table("Sponsors").AddColumn("IdType").AsString(400).Nullable();
            }
            if (!Schema.Table("Sponsors").Column("IdNumber").Exists())
            {
                Alter.Table("Sponsors").AddColumn("IdNumber").AsString(200).Nullable();
            }
            if (!Schema.Table("InvoiceDetails").Column("KzFirmName").Exists())
            {
                Alter.Table("InvoiceDetails").AddColumn("KzFirmName").AsString(700).Nullable();
            }
            if (!Schema.Table("InvoiceDetails").Column("IntermediaryDeatilId").Exists())
            {
                Alter.Table("InvoiceDetails").AddColumn("IntermediaryDeatilId").AsInt32().Nullable();
            }
            if (!Schema.Table("InvoiceDetails").Column("DueUtc").Exists())
            {
                Alter.Table("InvoiceDetails").AddColumn("DueUtc").AsDateTime().Nullable();
            }
            if (!Schema.Table("InvoiceDetails").Column("CurrencyId").Exists())
            {
                Alter.Table("InvoiceDetails").AddColumn("CurrencyId").AsInt32().Nullable();
            }
            if (!Schema.Table("Users").Column("ProfilePic").Exists())
            {
                Alter.Table("Users").AddColumn("ProfilePic").AsBoolean().Nullable();
            }
            if (!Schema.Table("EventDetails").Column("AltId").Exists())
            {
                Alter.Table("EventDetails").AddColumn("AltId").AsGuid().Nullable();
            }
            if (!Schema.Table("BankDetails").Column("IsIntermediaryBank").Exists())
            {
                Alter.Table("BankDetails").AddColumn("IsIntermediaryBank").AsBoolean().Nullable();
            }
        }
    }
}
