using System;

namespace FIL.Caching.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OwnerAttribute : Attribute
    {
        public Type ParentType { get; set; }

        public OwnerAttribute(Type parentType)
        {
            ParentType = parentType;
        }
    }
}