using FluentMigrator;
using FIL.Database.Migration.Core.Profiles;
using FIL.Database.Migration.Data.TestData;

namespace FIL.Database.Migration.Profiles
{
    [Profile("Default")]
    public class DefaultProfile : BaseProfile
    {
        public override void Up()
        {
            //Execute.Script("Profiles\\RemoveFunctions.sql");
            //Execute.Script("Profiles\\RemoveViews.sql");
            //Execute.Script("Profiles\\RemoveStoredProcedures.sql");
            //Execute.Script("Profiles\\RemoveTriggers.sql");

            base.Up();

            // Reload all test data 
            if (ConfigSet != Core.Enums.ConfigSet.Production)
            {
                DeleteTestData();
                InsertTestData();
            }
        }

        public override void Down()
        {
        }

        // Truncate each table with test data in order of foreign key dependencies
        // This is effectively the reverse order of the InsertTestData function
        private void DeleteTestData()
        {
            // Remove the test data and all data created by test users
        }

        // Insert test data in reverse order of foreign key dependencies
        private void InsertTestData()
        {
            // DO NOT USE UNTIL REMOVE SCRIPT WRITTEN
            Insert.TestVenueRows();
        }
    }
}
