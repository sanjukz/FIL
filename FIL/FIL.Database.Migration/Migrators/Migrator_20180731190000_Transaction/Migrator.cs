using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180731190000_Transaction
{
    [Migration(2018, 07, 31, 19, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {   
            Create.Index()
                .OnTable("Transactions")
                .OnColumn("AltId");
        }
    }
}
