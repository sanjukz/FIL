using FluentMigrator;
using FIL.Database.Migration.Core.Migrators;
using System.IO;

namespace FIL.Database.Migration.Core.Profiles
{
    [Profile("Default")]
    public class BaseProfile : BaseMigrator
    {
        public override void Up()
        {
            // Generate all functions based on latest version
            if (Directory.Exists("..\\..\\Functions\\Scalar"))
            {
                var scalarFunctions = Directory.EnumerateFiles("..\\..\\Functions\\Scalar");
                foreach (var filename in scalarFunctions)
                {
                    Execute.Script(filename);
                }
            }

            // Generate all views based on latest version
            if (Directory.Exists("..\\..\\Views"))
            {
                var views = Directory.EnumerateFiles("..\\..\\Views");
                foreach (var filename in views)
                {
                    Execute.Script(filename);
                }
            }

            // Generate all stored procedures based on latest version
            if (Directory.Exists("..\\..\\StoredProcedures"))
            {
                var storedProcedures = Directory.EnumerateFiles("..\\..\\StoredProcedures");
                foreach (var filename in storedProcedures)
                {
                    Execute.Script(filename);
                }
            }

            // Generate all triggers based on latest version
            if (Directory.Exists("..\\..\\Triggers"))
            {
                var triggers = Directory.EnumerateFiles("..\\..\\Triggers");
                foreach (var filename in triggers)
                {
                    Execute.Script(filename);
                }
            }
        }

        public override void Down()
        {
            // Unused.
        }
    }
}