using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class SearchVenue : IId<int>
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public EventSource EventSource { get; set; }
        public string TravelDuration { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string Currency { get; set; }
        public string CategoryName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public decimal Price { get; set; }
        public decimal PriceInDollar { get; set; }
        public string EventName { get; set; }
        public string CurrencyExchangeRate { get; set; }
        public long EventId { get; set; }
        public string PlaceVisitDate { get; set; }
        public Guid EventAltId { get; set; }
        public string EventDescription { get; set; }
        public string EventSlug { get; set; }
        public string Image { get; set; }
        public string PlaceVisitDuration { get; set; }
        public TravelSpeed Speed { get; set; }
        public long AdultETAId { get; set; }
        public long ChildETAId { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public List<PlaceWeekOff> PlaceOffDaysList { get; set; }
        public List<PlaceWeekOpenDays> PlaceWeekOpenDays { get; set; }
        public List<DayTimeMappings> DayTimeMappings { get; set; }
        public List<PlaceHolidayDate> PlaceHolidaysList { get; set; }
        public List<EventDetail> EventDetails { get; set; }
        public List<EventTicketDetail> EventTicketDetails { get; set; }
        public List<EventTicketAttribute> EventTicketAttributes { get; set; }
        public List<TicketCategory> ticketCategories { get; set; }
        public string Address { get; set; }
        public string CountryName { get; set; }
    }

    public class SearchVenueValidator : AbstractValidator<SearchVenue>, IKzValidator
    {
        public SearchVenueValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("name is required");
        }
    }
}