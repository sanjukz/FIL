using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180209180000_HotTickets
{
    [Migration(2018, 02, 09, 18, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("EventItems")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(128)
                .WithColumn("EventDate").AsDateTime()
                .WithColumn("MinPrice").AsInt32()
                .WithColumn("MaxPrice").AsInt32()
                .WithColumn("Address").AsString()
                .WithColumn("City").AsString()
                .WithColumn("State").AsString()
                .WithColumn("Country").AsString()
                .WithColumn("Type").AsString()
                .WithColumn("Currency").AsString()
                .WithColumn("ImagePath").AsString()
                .WithColumn("RSVPImage").AsString()
                .WithColumn("IsEnabled").AsBoolean()
                .WithColumn("CreatedUtc").AsDateTime()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
