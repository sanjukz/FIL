using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190216100000_EventHistory
{
    [Migration(2019, 02, 16, 10, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("EventHistories").Column("IsFeel").Exists())
            {
                Alter.Table("EventHistories").AddColumn("IsFeel").AsBoolean().Nullable();
            }

            if (!Schema.Table("EventHistories").Column("EventSourceId").Exists())
            {
                Alter.Table("EventHistories").AddColumn("EventSourceId").AsInt16().Nullable();
            }

            if (!Schema.Table("EventHistories").Column("Slug").Exists())
            {
                Alter.Table("EventHistories").AddColumn("Slug").AsString(500).Nullable();
            }

            if (!Schema.Table("EventHistories").Column("IsDelete").Exists())
            {
                Alter.Table("EventHistories").AddColumn("IsDelete").AsBoolean().Nullable();
            }

            if (!Schema.Table("EventHistories").Column("IsCreatedFromFeelAdmin").Exists())
            {
                Alter.Table("EventHistories").AddColumn("IsCreatedFromFeelAdmin").AsBoolean().Nullable();
            }
        }
    }
}
