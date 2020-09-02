using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190719151000_EventFeeTypeMappings
{
	[Migration(2019, 07, 19, 15, 10, 0)]
	public class Migrator: BaseMigrator
	{
		public override void Up()
		{
			Create.Table("EventFeeTypeMappings")
				.WithColumn("Id").AsInt32().PrimaryKey().Identity()
				.WithColumn("EventId").AsInt32().ForeignKey("Events", "Id")
				.WithColumn("ChannelId").AsInt32().Nullable()
				.WithColumn("FeeId").AsInt32().Nullable()
				.WithColumn("ValueTypeId").AsInt32().Nullable()
				.WithColumn("Value").AsDecimal().Nullable()
				.WithColumn("CreatedUtc").AsDateTime()
				.WithColumn("UpdatedUtc").AsDateTime().Nullable()
				.WithColumn("CreatedBy").AsGuid()
				.WithColumn("UpdatedBy").AsGuid().Nullable();
		}
	}
}
