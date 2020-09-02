using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20200326180000_TicketCategoryDetail
{
    [Migration(2020, 03, 26, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {

            Create.Table("TicketCategoryDetails")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("TicketCategoryId").AsInt32().ForeignKey("TicketCategories", "Id")
                  .WithColumn("Description").AsString(int.MaxValue).Nullable()
                  .WithColumn("Quantity").AsInt32()
                  .WithColumn("Channel").AsInt16().ForeignKey("Channels", "Id")
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
