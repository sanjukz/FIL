using System;

namespace FIL.Contracts.Attributes
{
    public class FeeTypeDataAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public FeeTypeDataAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}