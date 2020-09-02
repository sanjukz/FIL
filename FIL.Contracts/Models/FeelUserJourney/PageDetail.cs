namespace FIL.Contracts.Models
{
    public class PageDetail
    {
        public FIL.Contracts.Enums.PageType PageType { get; set; }
        public string PagePath { get; set; }
        public bool IsCategoryLevel { get; set; }
        public bool IsSubCategoryLevel { get; set; }
        public bool IsCountryLevel { get; set; }
        public bool IsStateLevel { get; set; }
        public bool IsCityLevel { get; set; }
    }
}