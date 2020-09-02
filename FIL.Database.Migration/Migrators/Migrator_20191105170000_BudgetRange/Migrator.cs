using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20191105170000_BudgetRange
{
    [Migration(2019, 11, 05, 17, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("MasterBudgetRanges")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("CurrencyId").AsInt32().ForeignKey("Currencytypes", "Id")
                  .WithColumn("MinPrice").AsDecimal(18, 2).Nullable()
                  .WithColumn("MaxPrice").AsDecimal(18, 2).Nullable()
                  .WithColumn("SortOrder").AsInt32().Nullable()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
