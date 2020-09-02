using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.FeelUserJourney;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers
{
    public interface IPlaceProvider
    {
        List<PlaceDetail> GetAllPlaces(FeelUserJourneyQuery query,
            List<FIL.Contracts.DataModels.EventCategory> subCategories,
            FIL.Contracts.DataModels.EventCategory eventCategory,
            FIL.Contracts.Enums.MasterEventType masterEventType,
            bool isCountryLevel = false,
            bool isStateLevel = false,
            bool isCityLevel = false);
    }

    public class PlaceProvider : IPlaceProvider
    {
        private IEventRepository _eventRepository;

        public PlaceProvider(
            IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public List<PlaceDetail> GetAllPlaces(FeelUserJourneyQuery query,
            List<FIL.Contracts.DataModels.EventCategory> subCategories,
            FIL.Contracts.DataModels.EventCategory eventCategory,
            FIL.Contracts.Enums.MasterEventType masterEventType,
            bool isCountryLevel = false,
            bool isStateLevel = false,
            bool isCityLevel = false)
        {
            List<PlaceDetail> placeDetails = new List<PlaceDetail>();
            // If it's all Online or IRL experience
            if (query.MasterEventType == Contracts.Enums.MasterEventType.Online || query.MasterEventType == Contracts.Enums.MasterEventType.InRealLife)
            {
                placeDetails = _eventRepository.GetAllPlaceDetailsByCategoryPage(subCategories.Select(s => s.Id).ToList()).ToList();
            }
            // If Global Category
            if (query.CityId == 0 && query.StateId == 0 && query.CountryId == 0 && query.PageType == Contracts.Enums.PageType.Category)
            {
                var subCat = subCategories.Take(4);
                if (eventCategory.MasterEventTypeId == Contracts.Enums.MasterEventType.Online)
                {
                    subCat = subCategories;
                }
                placeDetails = _eventRepository.GetAllPlaceDetailsByCategoryPage(subCat.Select(s => s.Id).ToList()).ToList();
            }
            // If Category or Subcategory country
            else if (isCountryLevel && query.PageType == Contracts.Enums.PageType.Category ||
                (isCountryLevel && !isStateLevel && !isCityLevel && query.PageType == Contracts.Enums.PageType.Country && (query.CategoryId != 0 || query.SubCategoryId != 0)))
            {
                placeDetails = _eventRepository.GetAllPlaceDetailsByCountry(subCategories.Select(s => s.Id).ToList(), query.CountryId).ToList();
            }
            // If Category or Subcategory state
            else if ((isStateLevel && query.PageType == Contracts.Enums.PageType.Category) ||
                (isStateLevel && query.PageType == Contracts.Enums.PageType.Country && (query.CategoryId != 0 || query.SubCategoryId != 0)))
            {
                placeDetails = _eventRepository.GetAllPlaceDetailsByState(subCategories.Select(s => s.Id).ToList(), query.StateId).ToList();
            }
            // If Category or Subcategory city
            else if ((isCityLevel && query.PageType == Contracts.Enums.PageType.Category) ||
                (isCityLevel && query.PageType == Contracts.Enums.PageType.Country && (query.CategoryId != 0 || query.SubCategoryId != 0)))
            {
                placeDetails = _eventRepository.GetAllPlaceDetailsByCity(subCategories.Select(s => s.Id).ToList(), query.CityId).ToList();
            }
            // If Global Country
            else if (query.PageType == Contracts.Enums.PageType.Country && query.CategoryId == 0 && query.SubCategoryId == 0 && !isStateLevel && !isCityLevel)
            {
                placeDetails = _eventRepository.GetAllPlaceDetailsByCountry(query.CountryId).ToList();
            }
            // If Global State
            else if (query.PageType == Contracts.Enums.PageType.Country && query.CategoryId == 0 && query.SubCategoryId == 0 && isStateLevel && !isCityLevel)
            {
                placeDetails = _eventRepository.GetAllPlaceDetailsByState(query.StateId).ToList();
            }
            // If Global City
            else if (query.PageType == Contracts.Enums.PageType.Country && query.CategoryId == 0 && query.SubCategoryId == 0 && !isStateLevel && isCityLevel)
            {
                placeDetails = _eventRepository.GetAllPlaceDetailsByCity(query.CityId).ToList();
            }
            //For Live Online Events Duration
            if (eventCategory != null && eventCategory.MasterEventTypeId == Contracts.Enums.MasterEventType.Online && placeDetails.Count > 0)
            {
                foreach (var item in placeDetails)
                {
                    try
                    {
                        if (item.InteractivityStartDateTime != null && item.EventFrequencyType == EventFrequencyType.OnDemand)
                        {
                            var timediff = RoundUp((DateTime)item.InteractivityStartDateTime, TimeSpan.FromMinutes(10)).Subtract(RoundUp(item.EventStartDateTime, TimeSpan.FromMinutes(10)));
                            string duration = string.Format("{0}:{1}", timediff.Hours, timediff.Minutes);
                            item.Duration = duration;
                        }
                        else
                        {
                            var timediff = RoundUp(item.EventEndDateTime, TimeSpan.FromMinutes(10)).Subtract(RoundUp(item.EventStartDateTime, TimeSpan.FromMinutes(10)));
                            string duration = string.Format("{0}:{1}", timediff.Hours, timediff.Minutes);
                            item.Duration = duration;
                        }
                    }
                    catch (Exception e)
                    {
                        var timediff = RoundUp(item.EventEndDateTime, TimeSpan.FromMinutes(10)).Subtract(RoundUp(item.EventStartDateTime, TimeSpan.FromMinutes(10)));
                        string duration = string.Format("{0}:{1}", timediff.Hours, timediff.Minutes);
                        item.Duration = duration;
                    }
                }
            }
            return placeDetails;
        }

        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
    }
}