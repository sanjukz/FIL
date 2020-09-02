using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190416110000_FeelBarcodeMapping
{
    [Migration(2019, 04, 16, 11, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("FeelBarcodeMappings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()                                
                .WithColumn("TransactionDetailId").AsInt64().ForeignKey("TransactionDetails", "Id")
				.WithColumn("Barcode").AsString().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();            
        }
    }
}