using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180201150000_SubscribeNewsLetter
{
    
        [Migration(2018, 02, 01, 15, 0, 0)]
        public class Migrator : BaseMigrator
        {
            public override void Up()
            {
                Create.Table("NewsLetterSignUps")
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("Email").AsString(128)
                    .WithColumn("IsEnabled").AsBoolean()
                    .WithColumn("CreatedUtc").AsDateTime()
                    .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid()
                    .WithColumn("UpdatedBy").AsGuid().Nullable();
            }
        }
    
}
