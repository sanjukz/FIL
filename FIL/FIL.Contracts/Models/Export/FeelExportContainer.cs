using System;

namespace FIL.Contracts.Models.Export
{
    public class FeelExportContainer
    {
        public long Id { get; set; }
        public int SiteId { get; set; }
        public Guid ParentId { get; set; }
        public string ParentName { get; set; }
        public string ParentDescription { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string CategoryName { get; set; }
        public string ParentImage1 { get; set; }
        public string ParentUrl { get; set; }
        public decimal Price { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public string Slug { get; set; }
    }
}