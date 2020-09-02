using System;
using System.Collections.Generic;
using System.Text;
using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;
namespace FIL.Database.Migration.Migrator_20181024190000_CorporateRequests
{
    [Migration(2018, 10, 24, 19, 0, 0)]
   public class Migrator:BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("UserEventTicketAttributeMappings").Exists())
            {
                Create.Table("UserEventTicketAttributeMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64().ForeignKey("Users", "Id")
                .WithColumn("EventTicketAttributeId").AsInt64().ForeignKey("EventTicketAttributes", "Id")
                .WithColumn("TicketLimit").AsInt32().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    }
}
