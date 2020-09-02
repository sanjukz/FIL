using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Enums;
using System;
using System.Text.RegularExpressions;

namespace FIL.Database.Migration.Core.Migrators
{
    public abstract class BaseMigrator : FluentMigrator.Migration
    {
        protected ConfigSet ConfigSet
        {
            get
            {
                ConfigSet configSet;
                var configValue = Runner.Configuration[Constants.ConfigSetNameEnvironmentVariable]
                    ?? Runner.Configuration[Constants.ConfigSetNameProperty];
                if (Enum.TryParse(configValue, true, out configSet))
                {
                    return configSet;
                }
                return ConfigSet.Default;
            }
        }

        private string BasePath { get; set; }

        protected void SetBasePath(Type instance)
        {
            string classNamespace = instance.Namespace;

            if (!string.IsNullOrWhiteSpace(classNamespace))
            {
                var nsArray = classNamespace.Split(Convert.ToChar("."));
                var ns = nsArray[nsArray.Length - 1];

                var migrationRegex = new Regex(@"Migrator_(\d{14})");
                var migrationMatch = migrationRegex.Match(ns);
                if (migrationMatch.Success)
                {
                    long result;
                    if (long.TryParse(migrationMatch.Groups[1].Value, out result))
                    {
                        var migrationAttribute = (MigrationAttribute)Attribute.GetCustomAttribute(instance, typeof(MigrationAttribute));
                        if (migrationAttribute.MigrationVersion == result)
                        {
                            BasePath = $"..\\..\\Migrators\\{ns}\\Scripts\\";
                        }
                        else
                        {
                            throw new ArgumentException("There is a mismatch between attribute value and the namespace. They must be equal.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Unable to parse namespace.");
                    }
                }
                else
                {
                    throw new ArgumentException("Namespace is in the improper form: Migrator_YYYYMMDDHHMMSS");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(instance));
            }
        }

        protected string GeneratePath(string subPath, string fileName)
        {
            return $"{BasePath}{subPath}{fileName}";
        }

        protected string GeneratePath(string fileName)
        {
            return $"{BasePath}{fileName}";
        }

        public override void Down()
        {
            // Don't require downs.
        }
    }
}