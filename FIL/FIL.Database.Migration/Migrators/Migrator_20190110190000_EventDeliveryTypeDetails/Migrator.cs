using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190110190000_EventDeliveryTypeDetails
{
    [Migration(2019, 01, 10, 19, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("EventDeliveryTypeDetails").Column("RefundPolicy").Exists())
            {
                Alter.Table("EventDeliveryTypeDetails").AddColumn("RefundPolicy").AsInt64().ForeignKey("RefundPolicies", "Id").Nullable();
            }
        }
    }
}
