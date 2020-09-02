using System;
using System.Collections.Generic;
using System.Text;
using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190213190000_Registrationeventusermappings
{
    [Migration(2019, 02, 13, 19, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("RegistrationEventUserMappings").Exists())
            {
                Create.Table("RegistrationEventUserMappings")
                    .WithColumn("Id").AsInt64()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("FirstName").AsString(40).Nullable()
                    .WithColumn("LastName").AsString(40).Nullable()
                    .WithColumn("Age").AsInt64()
                    .WithColumn("ParentFirstName").AsString(40).Nullable()
                    .WithColumn("ParentLastName").AsString(40).Nullable()
                    .WithColumn("Email").AsString(256).Nullable()
                    .WithColumn("PhoneNumber").AsString(40).Nullable()
                    .WithColumn("Address").AsString(2000).Nullable()
                    .WithColumn("Suburb").AsString(1600).Nullable()
                    .WithColumn("State").AsString(1600).Nullable()
                    .WithColumn("Zipcode").AsString(1000).Nullable()
                    .WithColumn("CountryId").AsInt32().ForeignKey("Countries", "Id")
                   .WithColumn("IsEnabled").AsBoolean()
                   .WithColumn("CreatedUtc").AsDateTime()
                   .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                   .WithColumn("CreatedBy").AsGuid()
                   .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
            else if (!Schema.Table("RegistrationEventUserMappings").Column("TransactionId").Exists())
            {
                Alter.Table("RegistrationEventUserMappings").AddColumn("TransactionId").AsInt64().ForeignKey("Transactions", "Id").Nullable();
            }
        }
    }
}
