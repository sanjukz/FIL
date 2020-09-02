using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180319120000_TicketFeeDetails
{
    [Migration(2018, 03, 19, 12, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_TicketFeeDetails_DiscountId_Discounts_Id")
                .OnTable("TicketFeeDetails");

            Delete.ForeignKey("FK_TicketFeeDetails_DiscountValueTypeId_DiscountValueTypes_Id")
                .OnTable("TicketFeeDetails");

            Rename.Column("DiscountId")
                .OnTable("TicketFeeDetails")
                .To("FeeId");

            Rename.Column("DiscountValueTypeId")
                .OnTable("TicketFeeDetails")
                .To("ValueTypeId");

            Rename.Column("DiscountGroupId")
                .OnTable("TicketFeeDetails")
                .To("FeeGroupId");

            Alter.Table("TicketFeeDetails")
                .AlterColumn("FeeId")
                .AsInt16()
                .NotNullable();

            Create.ForeignKey()
                .FromTable("TicketFeeDetails").ForeignColumn("FeeId")
                .ToTable("FeeTypes").PrimaryColumn("Id");

            Create.ForeignKey()
                .FromTable("TicketFeeDetails").ForeignColumn("ValueTypeId")
                .ToTable("ValueTypes").PrimaryColumn("Id");
        }
    }
}
