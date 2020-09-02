using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20191105180000_TournamentSectionFeeDetails
{
    [Migration(2019, 11, 05, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("TournamentSectionFeeDetails")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("TournamentLayoutSectionId").AsInt32().ForeignKey("TournamentLayoutSections", "Id")
                .WithColumn("FeeId").AsInt16().ForeignKey("FeeTypes", "Id")
                .WithColumn("ChannelId").AsInt16().ForeignKey("Channels", "Id")
                .WithColumn("DisplayName").AsString(100).Nullable()
                .WithColumn("ValueTypeId").AsInt16().ForeignKey("ValueTypes", "Id")
                .WithColumn("Value").AsDecimal()
                .WithColumn("FeeGroupId").AsInt32().Nullable()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
