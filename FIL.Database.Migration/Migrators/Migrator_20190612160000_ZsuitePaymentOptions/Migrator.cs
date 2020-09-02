using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190612160000_ZsuitePaymentOptions
{
    [Migration(2019, 06, 12, 16, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("ZsuitePaymentOptions").Exists())
            {
                Create.Table("ZsuitePaymentOptions")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100)
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("ZsuiteUserFeeDetails").Exists())
            {
                Create.Table("ZsuiteUserFeeDetails")
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("BoxOfficeUserAdditionalDetailId").AsInt64().ForeignKey("BoxofficeUserAdditionalDetails", "Id")
                  .WithColumn("ZsuitePaymentOptionId").AsInt32().ForeignKey("zsuitepaymentoptions", "Id")
                  .WithColumn("Fee").AsDecimal()
                  .WithColumn("IsEnabled").AsBoolean()
                  .WithColumn("CreatedUtc").AsDateTime()
                  .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsGuid()
                  .WithColumn("UpdatedBy").AsGuid().Nullable();
            }

            if (!Schema.Table("boxofficeuseradditionaldetails").Column("IsBillExpressUser").Exists())
            {
                Alter.Table("boxofficeuseradditionaldetails").AddColumn("IsBillExpressUser").AsBoolean().Nullable();
            }
        }
    }
}