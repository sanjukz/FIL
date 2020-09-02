using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20191107120000_BookingLogic_Change
{
    [Migration(2019, 11, 07, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
         {
            if (!Schema.Table("EventTicketDetails").Column("InventoryTypeId").Exists())
            {
                Alter.Table("EventTicketDetails").AddColumn("InventoryTypeId").AsInt16().ForeignKey("InventoryTypes", "Id").Nullable();
            }
            if (Schema.Table("MatchSeatTicketDetails").Column("MatchLayoutSectionSeatId").Exists())
            {
                Alter.Table("MatchSeatTicketDetails").AlterColumn("MatchLayoutSectionSeatId").AsInt64().Nullable();
            }
            if (!Schema.Table("MatchLayoutSections").Column("EventTicketDetailId").Exists())
            {
                Alter.Table("MatchLayoutSections").AddColumn("EventTicketDetailId").AsInt64().ForeignKey("EventTicketDetails", "Id").Nullable();
            }
            if (!Schema.Table("MatchLayoutSectionSeats").Column("SeatStatusId").Exists())
            {
                Alter.Table("MatchLayoutSectionSeats").AddColumn("SeatStatusId").AsInt16().ForeignKey("SeatStatuses", "Id").Nullable();
            }
            if (!Schema.Table("Events").Column("Url").Exists())
            {
                Alter.Table("Events").AddColumn("Url").AsString(int.MaxValue).Nullable();
            }
            if (!Schema.Table("MatchLayoutSectionSeats").Column("EventTicketDetailId").Exists())
            {
                Alter.Table("MatchLayoutSectionSeats").AddColumn("EventTicketDetailId").AsInt64().ForeignKey("EventTicketDetails", "Id").Nullable();
            }
            if (!Schema.Table("TransactionReleasedLogs").Exists())
            {
                Create.Table("TransactionReleasedLogs")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("TransactionId").AsInt64()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
            if (!Schema.Table("MasterLayoutCompanionSeatMappings").Exists())
            {
                Create.Table("MasterLayoutCompanionSeatMappings")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("WheelChairSeatId").AsInt64()
                .WithColumn("CompanionSeatId").AsInt64()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
            if (!Schema.Table("TournamentLayoutCompanionSeatMappings").Exists())
            {
                Create.Table("TournamentLayoutCompanionSeatMappings")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("WheelChairSeatId").AsInt64()
                .WithColumn("CompanionSeatId").AsInt64()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
            if (!Schema.Table("MatchLayoutCompanionSeatMappings").Exists())
            {
                Create.Table("MatchLayoutCompanionSeatMappings")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("WheelChairSeatId").AsInt64()
                .WithColumn("CompanionSeatId").AsInt64()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    }
}
