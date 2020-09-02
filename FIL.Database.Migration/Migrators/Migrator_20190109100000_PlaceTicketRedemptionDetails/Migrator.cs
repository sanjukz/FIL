using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190109100000_PlaceTicketRedemptionDetails
{

    [Migration(2019, 01, 09, 10, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("PlaceTicketRedemptionDetails")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
               .WithColumn("RedemptionsInstructions").AsString(int.MaxValue).Nullable()
               .WithColumn("RedemptionsAddress").AsString(int.MaxValue).Nullable()
               .WithColumn("RedemptionsCity").AsString(int.MaxValue).Nullable()
               .WithColumn("RedemptionsState").AsString(int.MaxValue).Nullable()
               .WithColumn("RedemptionsCountry").AsString(int.MaxValue).Nullable()
               .WithColumn("RedemptionsZipcode").AsString(int.MaxValue).Nullable()
               .WithColumn("RedemptionsDateTime").AsString(int.MaxValue).Nullable()
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
