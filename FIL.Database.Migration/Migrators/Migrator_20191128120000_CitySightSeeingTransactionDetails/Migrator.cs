using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20191128120000_CitySightSeeingTransactionDetails
{
    [Migration(2019, 11, 28, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("CitySightSeeingTransactionDetails")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("TransactionId").AsInt64()
                  .WithColumn("AltId").AsGuid()
                  .WithColumn("TicketId").AsString(200).Nullable()
                  .WithColumn("BookingDistributorReference").AsString(200).Nullable()
                  .WithColumn("BookingReference").AsString(200).Nullable()
                  .WithColumn("ReservationReference").AsString(200).Nullable()
                  .WithColumn("ReservationDistributorReference").AsString(200).Nullable()
                  .WithColumn("ReservationValidUntil").AsString(200).Nullable()
                  .WithColumn("HasTimeSlot").AsString(200).Nullable()
                  .WithColumn("FromDateTime").AsString(200).Nullable()
                  .WithColumn("EndDateTime").AsString(200).Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();

        }
    }
}
