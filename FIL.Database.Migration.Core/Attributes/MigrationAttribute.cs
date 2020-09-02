using System;

namespace FIL.Database.Migration.Core.Attributes
{
    public class MigrationAttribute : FluentMigrator.MigrationAttribute
    {
        public long MigrationVersion { get; private set; }

        public MigrationAttribute(int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(year, month, day, hour, minute, second))
        {
            try
            {
                MigrationVersion = CalculateValue(year, month, day, hour, minute, second);
            }
            catch (Exception)
            {
                throw new Exception("The date you input needs to resolve to an actual date.");
            }
        }

        private static long CalculateValue(int year, int month, int day, int hour, int minute, int second)
        {
            return year * 10000000000L + month * 100000000L + day * 1000000L + hour * 10000L + minute * 100L + second;
        }
    }
}