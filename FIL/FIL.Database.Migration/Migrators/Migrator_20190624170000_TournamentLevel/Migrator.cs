using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Database.Migration.Migrators.Migrator_20190624170000_TournamentLevel
{
    [Migration(2019, 06, 24, 17, 0, 0)]
    public class ZSuiteSchema : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("TournamentLayoutSections").Column("IsPrintSeatNumber").Exists())
            {
                Alter.Table("TournamentLayoutSections").AddColumn("IsPrintSeatNumber").AsBoolean().Nullable();
            }
            if (!Schema.Table("TournamentLayoutSections").Column("IsPrintBigX").Exists())
            {
                Alter.Table("TournamentLayoutSections").AddColumn("IsPrintBigX").AsBoolean().Nullable();
            }
            if (!Schema.Table("TournamentLayoutSectionSeats").Column("Price").Exists())
            {
                Alter.Table("TournamentLayoutSectionSeats").AddColumn("Price").AsDecimal().Nullable();
            }
            if (!Schema.Table("MatchLayoutSectionSeats").Column("Price").Exists())
            {
                Alter.Table("MatchLayoutSectionSeats").AddColumn("Price").AsDecimal().Nullable();
            }
            if (!Schema.Table("Events").Column("IsTokenize").Exists())
            {
                Alter.Table("Events").AddColumn("IsTokenize").AsBoolean().Nullable();
            }
        }
    }
}
