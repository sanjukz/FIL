using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Database.Migration.Migrators.Migrator_20182410072800_UsersWebsiteInvite
{
    [Migration(2018, 10, 24, 07, 28, 0)]
    public class Migrator: BaseMigrator
    {
        public override void Down()
        {
            Delete.Table("UsersWebsiteInvites");
        }

        public override void Up()
        {
            if (!Schema.Table("UsersWebsiteInvites").Exists())
            {
                Create.Table("UsersWebsiteInvites").WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("UserEmail").AsString().NotNullable()
                    .WithColumn("WebsiteID").AsInt32().NotNullable()
                    .WithColumn("IsUsed").AsByte().WithDefaultValue(false)
                    .WithColumn("CreatedOn").AsDateTime().Nullable()
                    .WithColumn("UsedOn").AsDateTime().Nullable()
                    .WithColumn("ModifiedOn").AsDateTime().Nullable()
                    .WithColumn("CreatedBy").AsGuid().Nullable()
                    .WithColumn("ModifiedBy").AsGuid().Nullable()
                    .WithColumn("UserInviteCode").AsString(255).NotNullable();
            }
        }        
    }
}
