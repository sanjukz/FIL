using System;

namespace FIL.Contracts.Attributes
{
    public class AmenityDataAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string Slug { get; set; }
        public int Order { get; set; }
        public bool IsShow { get; set; }

        public AmenityDataAttribute(string displayName, string slug, int order = 0, bool isShow = false)
        {
            DisplayName = displayName;
            Slug = slug;
            Order = order;
            IsShow = isShow;
        }
    }
}