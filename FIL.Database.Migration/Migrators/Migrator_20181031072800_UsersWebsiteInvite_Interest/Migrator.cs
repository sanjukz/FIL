using FIL.Database.Migration.Core.Migrators;
using FIL.Database.Migration.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Database.Migration.Migrators.Migrator_20181031072800_UsersWebsiteInvite_Interest
{
    [Migration(2018, 10, 31, 07, 28, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Down()
        {
            Delete.Table("UsersWebsiteInvites_Interests");
        }

        public override void Up()
        {
            Create.Table("UsersWebsiteInvites_Interests").WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Nationality").AsInt32().NotNullable()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().Nullable()
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid().NotNullable()
                .WithColumn("ModifiedBy").AsGuid().Nullable();
        }
    }
}
