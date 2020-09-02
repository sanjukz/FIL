using Humanizer;
using FIL.Contracts.Attributes;
using FIL.Contracts.Exceptions;
using FIL.Database.Migration.Core.Migrators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FIL.Database.Migration.Core.Profiles
{
    public class EnumTableGenerator : BaseMigrator
    {
        public override void Up()
        {
            var assembly = typeof(GenerateTableAttribute).Assembly;

            foreach (Type type in assembly.GetTypes().Where(t => t.IsEnum))
            {
                var generateTableAttribute = type.GetCustomAttribute<GenerateTableAttribute>();
                if (generateTableAttribute != null)
                {
                    if (!string.IsNullOrWhiteSpace(generateTableAttribute.TableName))
                    {
                        if (string.IsNullOrWhiteSpace(generateTableAttribute.DescriptionColumnName))
                        {
                            throw new Exception($"DescriptionColumnName cannot be null on {type.Name}");
                        }
                        GenerateTableFromEnum(type, generateTableAttribute.TableName, generateTableAttribute.DescriptionColumnName);
                    }
                    else
                    {
                        GenerateTableFromEnum(type);
                    }
                }
            }
        }

        private void GenerateTableFromEnum(Type enumType)
        {
            GenerateTableFromEnum(enumType, enumType.Name.Pluralize(), enumType.Name);
        }

        private void GenerateTableFromEnum(Type enumType, string tableName, string descriptionColumnName)
        {
            if (enumType.GetCustomAttribute<FlagsAttribute>() == null)
            {
                GenerateRegularTable();
            }
            else
            {
                GenerateFlagsTable();
            }

            void GenerateRegularTable()
            {
                if (!Schema.Table(tableName).Exists())
                {
                    Create.Table(tableName)
                        .WithColumn("Id").AsInt16().PrimaryKey("PK_" + tableName)
                        .WithColumn(descriptionColumnName).AsString(32);
                }

                var ids = new List<int>();

                foreach (var value in Enum.GetValues(enumType))
                {
                    var name = Enum.GetName(enumType, value);
                    var id = (int)value;
                    ids.Add(id);

                    Execute.Sql($@"
                    IF EXISTS (SELECT 1 FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = '{tableName}')
                        SET IDENTITY_INSERT [dbo].[{tableName}] ON
                    IF EXISTS (SELECT 1 FROM {tableName} WHERE Id = {id})
                        UPDATE {tableName} SET {descriptionColumnName} = '{name}' WHERE Id = {id}
                    ELSE
                        INSERT INTO {tableName} (Id, {descriptionColumnName}) VALUES ({id}, '{name}')
                    IF EXISTS (SELECT 1 FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = '{tableName}')
                        SET IDENTITY_INSERT [dbo].[{tableName}] OFF
                ");
                }

                Execute.Sql($"DELETE FROM [{tableName}] WHERE Id NOT IN ({string.Join(",", ids)})");
            }

            void GenerateFlagsTable()
            {
                if (Enum.GetName(enumType, 0) != "None")
                {
                    throw new CustomException($"Must have a 0 value of None to GenerateTable the Flags enum {enumType}");
                }

                if (!Schema.Table(tableName).Exists())
                {
                    Create.Table(tableName)
                        .WithColumn("Id").AsInt16().PrimaryKey()
                        .WithColumn(descriptionColumnName).AsCustom("varchar(max)")
                        .WithColumn("Base").AsBoolean();
                }

                int id;
                for (id = 0; !(IsPowerOfTwo(id) && !Enum.IsDefined(enumType, id)); id++)
                {
                    var name = Enum.ToObject(enumType, id).ToString();

                    Execute.Sql($@"
                    IF EXISTS (SELECT 1 FROM {tableName} WHERE Id = {id})
                        UPDATE {tableName} SET {descriptionColumnName} = '{name}' WHERE Id = {id}
                    ELSE
                        INSERT INTO {tableName} (Id, {descriptionColumnName}, Base) VALUES ({id}, '{name}', {(IsPowerOfTwo(id) ? 1 : 0)})
                    ");
                }

                Execute.Sql($"DELETE FROM [{tableName}] WHERE Id >= {id}");
            }
        }

        private static bool IsPowerOfTwo(long x)
        {
            return (x & (x - 1)) == 0;
        }
    }
}