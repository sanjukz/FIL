using System;

namespace FIL.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class GenerateTableAttribute : Attribute
    {
        public string TableName { get; set; }
        public string DescriptionColumnName { get; set; }

        public GenerateTableAttribute(string tableName, string descriptionColumnName)
        {
            TableName = tableName;
            DescriptionColumnName = descriptionColumnName;
        }

        public GenerateTableAttribute()
        {
        }
    }
}