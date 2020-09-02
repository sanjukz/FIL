using System;
using System.Collections.Generic;
using System.Text;
using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190313110000_CitySightSeeing
{
    [Migration(2019, 03, 13, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("CitySightSeeingExtraOptions").Exists())
            {
                Create.Table("CitySightSeeingExtraOptions")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("ExtraOptionId").AsString(100).Nullable()
                    .WithColumn("ExtraOptionName").AsString(100).Nullable()
                    .WithColumn("ExtraOptionType").AsString(100).Nullable()
                    .WithColumn("IsMandatory").AsInt32().Nullable()                    
                    .WithColumn("CitySightSeeingTicketTypeDetailId").AsInt32().ForeignKey("CitySightSeeingTicketTypeDetails", "Id").Nullable()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("CitySightSeeingExtraSubOptions").Exists())
            {
                Create.Table("CitySightSeeingExtraSubOptions")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("AltId").AsGuid()
                    .WithColumn("SubOptionId").AsString(100).Nullable()
                    .WithColumn("SubOptionName").AsString(100).Nullable()
                    .WithColumn("SubOptionPrice").AsString(100).Nullable()
                    .WithColumn("CitySightSeeingExtraOptionId").AsInt32().ForeignKey("CitySightSeeingExtraOptions", "Id").Nullable()
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("CitySightSeeingEventDetailMappings").Exists())
            {
                Create.Table("CitySightSeeingEventDetailMappings")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("CitySightSeeingTicketId").AsInt32().ForeignKey("CitySightSeeingTickets", "Id")
                    .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("CitySightSeeingEventTicketDetailMappings").Exists())
            {
                Create.Table("CitySightSeeingEventTicketDetailMappings")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("CitySightSeeingTicketTypeDetailId").AsInt32().ForeignKey("CitySightSeeingTicketTypeDetails", "Id")
                    .WithColumn("EventTicketDetailId").AsInt64().ForeignKey("EventTicketDetails", "Id")
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    }
}
