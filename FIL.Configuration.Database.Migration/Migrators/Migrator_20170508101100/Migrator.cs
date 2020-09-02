using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Configuration.Database.Migration.Migrators.Migrator_20170508101100
{
    [Migration(2017, 5, 8, 10, 11, 0)]
    public class Migrator : BaseMigrator
    {
        public Migrator()
        {
            SetBasePath(GetType());
        }

        public override void Up()
        {
            Create.Table("ConfigurationSets")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("CanMigrate").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("ParentConfigurationSetId").AsInt32().Nullable().WithDefaultValue(null).ForeignKey("ConfigurationSets", "Id")
                .WithColumn("IsEnabled").AsBoolean().WithDefaultValue(true)
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("ConfigurationKeys")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();

            Create.Table("Configurations")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ConfigurationKeyId").AsInt32().NotNullable().ForeignKey("ConfigurationKeys", "Id")
                .WithColumn("ConfigurationSetId").AsInt32().NotNullable().ForeignKey("ConfigurationSets", "Id")
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("IsEnabled").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsGuid()
                .WithColumn("UpdatedBy").AsGuid().Nullable();
        }
    }
}