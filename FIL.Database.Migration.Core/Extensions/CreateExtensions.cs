using FluentMigrator.Builders.Create.Table;

namespace FIL.Database.Migration.Core.Extensions
{
    public static class CreateExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax WithAuditColumns(this ICreateTableColumnOptionOrWithColumnSyntax table)
        {
            return table
                .WithColumn("IsEnabled").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("CreatedUtc").AsDateTime().WithDefaultValue("getutcdate()")
                .WithColumn("UpdatedUtc").AsDateTime().Nullable()
                .WithColumn("CreatedBy").AsInt32().NotNullable()
                .WithColumn("UpdatedBy").AsInt32().Nullable();
        }
    }
}