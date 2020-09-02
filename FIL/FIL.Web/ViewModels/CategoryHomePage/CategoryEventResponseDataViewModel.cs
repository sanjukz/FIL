using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Web.Feel.ViewModels.Category;

namespace FIL.Web.Feel.ViewModels.CategoryHomePage {
    public class CategoryEventResponseDataViewModel {
        public CategoryViewModel Category { get; set; }
        public List<CategoryEventContainer> CategoryEvents { get; set; }
        public CountryDescription CountryDescription { get; set; }
        public FIL.Contracts.DataModels.CityDescription CityDescription { get; set; }
        public List<CountryContentMapping> CountryContentMapping { get; set; }
    }
    public class CategoryEventContainer {
        public CategoryEvent CategoryEvent { get; set; }
        public IEnumerable<City> City { get; set; }
        public IEnumerable<State> State { get; set; }
        public IEnumerable<FIL.Contracts.Models.Country> Country { get; set; }
        public Event Event { get; set; }
        public string EventType { get; set; }
        public string EventCategory { get; set; }
        public string LocalStartDateTime { get; set; }
        public string TimeZoneAbbrivation { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public IEnumerable<Venue> Venue { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<Rating> Rating { get; set; }
        public string ParentCategory { get; set; }
        public string Duration { get; set; }
        public List<string> EventCategories { get; set; }
        public FIL.Contracts.Enums.EventFrequencyType EventFrequencyType { get; set; }
        public FIL.Contracts.DataModels.LiveEventDetail LiveEventDetail { get; set; }
    }
}