using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.ValueRetail
{
    public class Image
    {
        public bool IsLead { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ThumbnailDesc { get; set; }
        public string StandardUrl { get; set; }
        public string StandardDesc { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidToDate { get; set; }
        public int MinCardOrderValue { get; set; }
        public int MaxCardOrderValue { get; set; }
        public int MaxNoOfCards { get; set; }
        public int DisplayOrder { get; set; }
        public IList<int> PreSetValue { get; set; }
        public IList<Image> Images { get; set; }
    }

    public class GiftCardAvailabilityResponse
    {
        public IList<Item> Items { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}