using System;

namespace FIL.Contracts.Attributes
{
    public class CategoryDataAttribute : Attribute
    {
        public int Order { get; set; }
        public string DisplayName { get; set; }
        public string Slug { get; set; }
        public bool IsHomePage { get; set; }
        public int CategoryId { get; set; }
        public bool IsFeel { get; set; }

        public CategoryDataAttribute(string displayName, string slug, int order = 0, bool isHomePage = false, int categoryId = 0, bool isFeel = false)
        {
            DisplayName = displayName;
            Slug = slug;
            IsHomePage = isHomePage;
            CategoryId = categoryId;
            IsFeel = isFeel;
            Order = order;
        }
    }
}