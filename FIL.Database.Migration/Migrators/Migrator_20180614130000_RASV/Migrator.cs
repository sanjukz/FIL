using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180614130000_RASV
{

    [Migration(2018, 06, 14, 13, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Create.Table("RASVTicketTypeMappings")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity()
               .WithColumn("EventDetailId").AsInt64().ForeignKey("EventDetails", "Id")
               .WithColumn("RASVTicketTypeId").AsInt16().ForeignKey("RASVTicketTypes", "Id")
               .WithColumn("IsEnabled").AsBoolean().Indexed()
               .WithColumn("CreatedUtc").AsDateTime()
               .WithColumn("UpdatedUtc").AsDateTime().Nullable()
               .WithColumn("CreatedBy").AsGuid()
               .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}
