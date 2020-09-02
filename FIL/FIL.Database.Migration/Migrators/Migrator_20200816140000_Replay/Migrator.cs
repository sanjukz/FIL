using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20200816140000_Replay
{
    [Migration(2020, 08, 16, 14, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("ReplayDetails")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("AltId").AsGuid()
                 .WithColumn("EventId").AsInt64().ForeignKey("Events", "Id")
                 .WithColumn("StartDate").AsDateTime()
                 .WithColumn("EndDate").AsDateTime()
                 .WithColumn("IsPaid").AsBoolean()
                 .WithColumn("CurrencyId").AsInt32().ForeignKey("CurrencyTypes", "Id").Nullable()
                 .WithColumn("Price").AsDecimal().Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}