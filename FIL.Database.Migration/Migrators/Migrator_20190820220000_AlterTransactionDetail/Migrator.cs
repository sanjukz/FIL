using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190820220000_AlterTransactionDetail
{
    [Migration(2019, 08, 20, 22, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {            
            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierTown").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierTown")
                .AsString()
                .Nullable();
            }
            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierAddress").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierAddress")
                .AsString()
                .Nullable(); 
            }
            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierZipcode").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierZipcode")
                .AsInt32()
                .Nullable();
            }
            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierRegion").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierRegion")
                .AsString()
                .Nullable();
            }
            if (!Schema.Table("TransactionDeliveryDetails").Column("CourierCountry").Exists())
            {
                Alter.Table("TransactionDeliveryDetails")
                .AddColumn("CourierCountry")
                .AsString()
                .Nullable();
            }
        }
    }
}
