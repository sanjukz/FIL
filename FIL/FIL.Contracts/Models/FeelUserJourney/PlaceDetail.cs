using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class PlaceDetail
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsTokenize { get; set; }
        public EventType EventTypeId { get; set; }
        public EventSource EventSource { get; set; }
        public MasterEventType MasterEventTypeId { get; set; }
        public long EventDetailId { get; set; }
        public int EventCategoryId { get; set; }
        public string ParentCategory { get; set; }
        public int ParentCategoryId { get; set; }
        public string ParentCategorySlug { get; set; }
        public string Category { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public Guid CountryAltId { get; set; }
        public string CountryName { get; set; }
        public string CategorySlug { get; set; }
        public string SubCategorySlug { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int Rating { get; set; }
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
        public List<string> SubCategories { get; set; }
        public string Url { get; set; }
        public string EventDescription { get; set; }
        public string Venue { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public DateTime EventStartDateTime { get; set; }
        public DateTime EventEndDateTime { get; set; }
        public string Duration { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
        public DateTime? InteractivityStartDateTime { get; set; }
    }
}