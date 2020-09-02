using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20200819170000_TransactionDetailScheduleMapping
{
    [Migration(2020, 08, 19, 17, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("TransactionScheduleDetails")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("TransactionDetailId").AsInt64().ForeignKey("TransactionDetails", "Id")
                 .WithColumn("ScheduleDetailId").AsInt64().ForeignKey("ScheduleDetails", "Id")
                 .WithColumn("StartDateTime").AsDateTime().Nullable()
                 .WithColumn("EndDateTime").AsDateTime().Nullable()
                 .WithColumn("IsEnabled").AsBoolean()
                 .WithColumn("CreatedUtc").AsDateTime()
                 .WithColumn("CreatedBy").AsGuid()
                 .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                 .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}