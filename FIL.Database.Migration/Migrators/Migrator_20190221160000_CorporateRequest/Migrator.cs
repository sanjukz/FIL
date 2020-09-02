using FIL.Database.Migration.Core.Migrators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Database.Migration.Migrators.Migrator_20190221160000_CorporateRequest
{
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("CorporateRequests").Column("Company").Exists())
            {
                Alter.Table("CorporateRequests").AddColumn("Company").AsString(1200).Nullable();
            }
        }
    }
}
