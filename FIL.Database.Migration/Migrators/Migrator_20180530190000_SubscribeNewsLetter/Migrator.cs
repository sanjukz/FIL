using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20180530190000_SubscribeNewsLetter
{

    [Migration(2018, 05, 30, 19, 0, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            Alter.Table("NewsLetterSignUps").AddColumn("IsFeel").AsBoolean().Nullable();

        }

    }

}
