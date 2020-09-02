using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.MIgrator_20180718180000_Redemption
{
    [Migration(2018, 07, 18, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("MatchSeatTicketDetails").Column("IsConsumed").Exists())
            {
                Alter.Table("MatchSeatTicketDetails").AddColumn("IsConsumed").AsBoolean().NotNullable().WithDefaultValue(false);
            }

            Create.Table("Tokens")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Token").AsGuid()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            if (!Schema.Table("EventTicketAttributes").Column("AdditionalInfo").Exists())
            {
                Alter.Table("EventTicketAttributes").AddColumn("AdditionalInfo").AsString(int.MaxValue).Nullable();
            }
        }

    }
}
