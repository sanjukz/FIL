using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.Venue;
using FIL.Contracts.Queries.GoogleAPIUtility;
using FIL.Foundation.Senders;
using FIL.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FIL.Contracts.Commands.GoogleAPIUtility;
using FIL.Configuration;
using MoreLinq;
using FIL.Web.Feel.ViewModels.EventCreation;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.ItineraryTicket;
using FIL.Contracts.QueryResults.ItineraryTicket;
using FIL.Web.Feel.Modules.SiteExtensions;
using System.Net.Http;
using System.Web;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Web.Feel.Providers;
using FIL.Web.Feel.Providers.Itinerary;
using FIL.Web.Feel.ViewModels.Itinerary;
using System.Collections;

namespace FIL.Web.Feel.Controllers
{
    public class PlaceItinerarySearchController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISettings _settings;
        private readonly ILogger _logger;
        private readonly IGeoCurrency _geoCurrency;
        private readonly ILatLongProvider _latLongProvider;
        private readonly IVisitDurationProvider _visitDurationProvider;
        private readonly INearestPlaceProvider _nearestPlaceProvider;
        private readonly IPlacePriceProvider _placePriceProvider;
        private readonly IDurationTimeProvider _durationTimeProvider;

        public PlaceItinerarySearchController(ICommandSender commandSender,
            IQuerySender querySender,
            ISettings settings,
            ILogger logger,
            ILatLongProvider latLongProvider,
            IVisitDurationProvider visitDurationProvider,
            INearestPlaceProvider nearestPlaceProvider,
            IPlacePriceProvider placePriceProvider,
            IDurationTimeProvider durationTimeProvider,
            IGeoCurrency geoCurrency)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _settings = settings;
            _logger = logger;
            _geoCurrency = geoCurrency;
            _latLongProvider = latLongProvider;
            _visitDurationProvider = visitDurationProvider;
            _nearestPlaceProvider = nearestPlaceProvider;
            _placePriceProvider = placePriceProvider;
            _durationTimeProvider = durationTimeProvider;
        }


        string GetDurationTime(Parent result)
        {
            String durationValue;
            if (result.rows[0].elements[0].status == "ZERO_RESULTS")
            {
                durationValue = "0 min";
            }
            else
            {
                durationValue = result.rows[0].elements[0].duration.text;
            }
            return durationValue;
        }

        TimeSpan GetCurrnetTimeSpan(string time)
        {
            var startTime = time.Split(":");
            var startTimeTimeSpan = new TimeSpan(Convert.ToInt16(startTime.ElementAt(0)), Convert.ToInt16(startTime.ElementAt(1)), Convert.ToInt16(startTime.ElementAt(2)));
            return startTimeTimeSpan;
        }

        SearchVenue GetLastPlace(SearchVenue lastPlace, List<SearchVenue> currentDayList)
        {
            if (lastPlace.Id == 0)
            {
                lastPlace = currentDayList.FirstOrDefault();
            }
            return lastPlace;
        }

        List<SearchVenue> PushVenue(SearchVenue cardModel, List<SearchVenue> dayByDayList, int position)
        {
            ArrayList arrayList = new ArrayList(dayByDayList);
            arrayList.Insert(Convert.ToInt16(position), cardModel);
            var currentDayList = arrayList.Cast<SearchVenue>().ToList();
            return currentDayList;
        }

        ItineraryBoardResponseViewModel RegulizeDayCalendar(List<List<SearchVenue>> searchModel,
            SearchVenue lastPlace,
            SearchVenue currentPlace,
            TimeSpan currentTime,
            string endTimeOfDay,
            bool isRandom = false)
        {
            var visitMinutes = 60;
            var visitTimes = new List<int>() { 0, 30, 45, 60 };
            if (isRandom)
            {
                visitMinutes = visitTimes.ElementAt(new Random().Next(4));
            }
            var result = _visitDurationProvider.GetDistance((lastPlace.Address), (currentPlace.Address));
            var durationTime = _durationTimeProvider.DurationTime(result);
            var durationValue = GetDurationTime(result);
            var currentDate = new DateTime(Convert.ToInt16(currentPlace.PlaceVisitDate.Split("-").ElementAt(0)), Convert.ToInt16(currentPlace.PlaceVisitDate.Split("-").ElementAt(1)), Convert.ToInt16(currentPlace.PlaceVisitDate.Split("-").ElementAt(2)));
            var currentDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, (int)currentTime.Hours, currentTime.Minutes, currentTime.Seconds);
            currentTime = currentTime.Add(new TimeSpan(0, durationTime.DurationTime + 30, 0));
            var totalMinutes = (int)(currentTime + new TimeSpan(0, 30, 0)).TotalMinutes;
            currentTime = new TimeSpan(0, totalMinutes - totalMinutes % 30, 0);
            var journeyDateTime = currentDateTime.AddMinutes(durationTime.DurationTime + 30); // Add Timespan of the total journey
            if (currentDateTime.Date != journeyDateTime.Date)
            {
                return new ItineraryBoardResponseViewModel
                {
                    Success = false,
                    IsSourceCountZero = true,
                    IsValidDandD = false,
                    IsTargetDateExceed = true,
                    ItineraryBoardData = searchModel
                };
            }
            if (currentTime.Add(new TimeSpan(1, visitMinutes, 0)) > TimeSpan.Parse(endTimeOfDay + ":00"))
            {
                return new ItineraryBoardResponseViewModel
                {
                    Success = false,
                    IsSourceCountZero = true,
                    IsValidDandD = false,
                    IsTargetDateExceed = true,
                    ItineraryBoardData = searchModel
                };
            }
            currentPlace.StartTime = currentTime.ToString();
            currentPlace.EndTime = currentTime.Add(new TimeSpan(1, visitMinutes, 0)).ToString();
            currentPlace.PlaceVisitDuration = durationValue;
            currentPlace.TravelDuration = durationValue;
            return new ItineraryBoardResponseViewModel
            {
                Success = true,
                IsSourceCountZero = false,
                IsValidDandD = false,
                IsTargetDateExceed = true,
                ItineraryBoardData = searchModel,
                CurrentPlace = currentPlace
            };
        }

        ItineraryBoardResponseViewModel CheckDiffCityDrag(List<List<SearchVenue>> searchModel, string sourceDate, string targetDate)
        {
            var placeSourceDayList = searchModel.Where(s => s.Any(p => p.PlaceVisitDate.Split("-").ElementAt(2) == sourceDate)).FirstOrDefault().FirstOrDefault();
            var placeTargetDayList = searchModel.Where(s => s.Any(p => p.PlaceVisitDate.Split("-").ElementAt(2) == targetDate)).FirstOrDefault().FirstOrDefault();
            if (placeSourceDayList.CityName != placeTargetDayList.CityName)
            {
                return new ItineraryBoardResponseViewModel
                {
                    Success = false,
                    IsSourceCountZero = true,
                    IsValidDandD = false,
                    IsDiffCityDandD = true,
                    IsTargetDateExceed = false,
                    ItineraryBoardData = searchModel
                };
            }
            return new ItineraryBoardResponseViewModel
            {
                Success = true,
                IsSourceCountZero = true,
                IsValidDandD = false,
                IsDiffCityDandD = false,
                IsTargetDateExceed = false,
                ItineraryBoardData = searchModel
            };
        }

        //XXX:TO: Code optimization and consolidation
        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/ItineraryBoard")]
        public async Task<ItineraryBoardResponseViewModel> GetItineraryBoardData([FromBody] ItineraryBoardViewModel model)
        {
            var searchVenueList = new List<List<SearchVenue>>();
            List<List<SearchVenue>> searchModel = new List<List<SearchVenue>>();
            searchModel.AddRange(model.ItineraryBoardCopyData.ToList());
            var lastPlace = new SearchVenue();
            var endTimeOfDay = "20";
            var index = -1;
            try
            {
                var sourcelane = model.IsAddNew ? null : model.RootObject.Lanes.Where(s => s.id == model.SourceLaneId).FirstOrDefault();
                var tagrgetlane = model.IsAddNew ? null : model.RootObject.Lanes.Where(s => s.id == model.TargetLaneId).FirstOrDefault();
                var sourceDate = model.IsAddNew ? null : sourcelane.label.Split(" ").FirstOrDefault();
                var targetDate = model.IsAddNew ? null : tagrgetlane.label.Split(" ").FirstOrDefault();
                var cardIndex = model.IsAddNew ? 0 : model.RootObject.Lanes.Where(s => s.Cards.Any(p => p.id == model.CardId)).FirstOrDefault().Cards.FindIndex(p => p.id == model.CardId);
                if (!model.IsAddNew && (model.SourceLaneId == model.TargetLaneId) && model.Position == cardIndex && !model.IsDelete)
                {
                    return new ItineraryBoardResponseViewModel
                    {
                        Success = true,
                        IsSourceCountZero = false,
                        IsValidDandD = false,
                        ItineraryBoardData = model.ItineraryBoardData
                    };
                }
                if (!model.IsAddNew && sourcelane.Cards.Count() == 0)
                {
                    return new ItineraryBoardResponseViewModel
                    {
                        Success = false,
                        IsSourceCountZero = true,
                        IsValidDandD = false,
                        ItineraryBoardData = model.ItineraryBoardData
                    };
                }
                if (!model.IsAddNew && Convert.ToInt16(sourceDate) < 10)
                {
                    sourceDate = "0" + sourceDate;
                }
                if (!model.IsAddNew && Convert.ToInt16(targetDate) < 10)
                {
                    targetDate = "0" + targetDate;
                }
                var cardModel = model.IsAddNew ? null : model.ItineraryBoardData.Where(s => s.Any(p => p.PlaceVisitDate.Split("-").ElementAt(2) == sourceDate)).FirstOrDefault().Where(s => s.Id == Convert.ToInt16(model.CardId)).FirstOrDefault();
                var responseModel = model.IsAddNew ? null : CheckDiffCityDrag(model.ItineraryBoardCopyData, sourceDate, targetDate);
                if (!model.IsAddNew && !responseModel.Success)
                {
                    return responseModel;
                }
                if (model.IsAddNew)
                {
                    var veneueResultFromQuerySender = await _querySender.Send(new SearchVenueQuery
                    {
                        PlaceId = model.PlaceId
                    });
                    var data = model.ItineraryBoardData.Where(s => s.Any(x => x.CityId == veneueResultFromQuerySender.Venues.FirstOrDefault().CityId));
                    var isAdded = false;
                    foreach (var dayByDayList in model.ItineraryBoardData)
                    {
                        var currentDayByDayList = new List<SearchVenue>();
                        if (dayByDayList.Any(s => s.CityId == veneueResultFromQuerySender.Venues.FirstOrDefault().CityId) && dayByDayList.Count() < 5 && !isAdded)
                        {
                            isAdded = true;
                            var currentDayList = dayByDayList;
                            var startTime = GetCurrnetTimeSpan(dayByDayList.FirstOrDefault().StartTime);
                            int rand = new Random().Next(dayByDayList.Count());
                            veneueResultFromQuerySender.Venues.FirstOrDefault().PlaceVisitDate = dayByDayList.FirstOrDefault().PlaceVisitDate;
                            currentDayList = PushVenue(veneueResultFromQuerySender.Venues.FirstOrDefault(), currentDayList, rand);
                            currentDayList.FirstOrDefault().StartTime = startTime.ToString();
                            currentDayList.FirstOrDefault().EndTime = startTime.Add(new TimeSpan(1, 0, 0)).ToString();
                            currentDayByDayList.Add(currentDayList.FirstOrDefault());
                            lastPlace = GetLastPlace(lastPlace, currentDayList);
                            var currentTime = startTime.Add(new TimeSpan(1, 0, 0));
                            foreach (var currentPlace in currentDayList.Skip(1))
                            {
                                var responseViewModel = RegulizeDayCalendar(model.ItineraryBoardCopyData, lastPlace, currentPlace, currentTime, endTimeOfDay, true);
                                if (!responseViewModel.Success)
                                {
                                    isAdded = false;
                                    currentDayByDayList = new List<SearchVenue>();
                                    currentDayByDayList.AddRange(model.ItineraryBoardCopyData.Where(s => s.Any(x => x.PlaceVisitDate == dayByDayList.FirstOrDefault().PlaceVisitDate)).First());
                                    break;
                                }
                                currentTime = GetCurrnetTimeSpan(responseViewModel.CurrentPlace.EndTime);
                                currentDayByDayList.Add(currentPlace);
                                lastPlace = currentPlace;
                            }
                        }
                        else
                        {
                            currentDayByDayList.AddRange(dayByDayList);
                        }
                        searchVenueList.Add(currentDayByDayList);
                    }
                    return new ItineraryBoardResponseViewModel
                    {
                        Success = isAdded,
                        IsSourceCountZero = false,
                        IsTargetDateExceed = false,
                        IsValidDandD = true,
                        ItineraryBoardData = searchVenueList
                    };
                }

                foreach (var dayByDayList in model.ItineraryBoardData)
                {
                    index += 1;
                    var currentDayByDayList = new List<SearchVenue>();
                    if (index == Convert.ToInt16(model.TargetLaneId) && !model.IsDelete)
                    {
                        cardModel.PlaceVisitDate = dayByDayList.FirstOrDefault().PlaceVisitDate;
                        var startTime = GetCurrnetTimeSpan(dayByDayList.FirstOrDefault().StartTime);

                        var currentDayList = dayByDayList;
                        if (model.SourceLaneId == model.TargetLaneId)
                        {
                            currentDayList = currentDayList.Where(s => s.Id != Convert.ToInt16(model.CardId)).ToList();
                        }
                        currentDayList = PushVenue(cardModel, currentDayList, model.Position);
                        currentDayList.FirstOrDefault().StartTime = startTime.ToString();
                        currentDayList.FirstOrDefault().EndTime = startTime.Add(new TimeSpan(2, 0, 0)).ToString();
                        currentDayByDayList.Add(currentDayList.FirstOrDefault());
                        lastPlace = GetLastPlace(lastPlace, currentDayList);
                        var currentTime = startTime.Add(new TimeSpan(2, 0, 0));
                        foreach (var currentPlace in currentDayList.Skip(1))
                        {
                            var responseViewModel = RegulizeDayCalendar(model.ItineraryBoardCopyData, lastPlace, currentPlace, currentTime, endTimeOfDay);
                            if (!responseViewModel.Success)
                            {
                                return responseViewModel;
                            }
                            currentTime = GetCurrnetTimeSpan(responseViewModel.CurrentPlace.EndTime);
                            currentDayByDayList.Add(currentPlace);
                            lastPlace = currentPlace;
                        }
                    }
                    if (index == Convert.ToInt16(model.SourceLaneId) && model.SourceLaneId != model.TargetLaneId && !model.IsDelete)
                    {
                        var startTime = GetCurrnetTimeSpan(dayByDayList.FirstOrDefault().StartTime);
                        lastPlace = GetLastPlace(lastPlace, dayByDayList);
                        var currentDayList = dayByDayList.Where(s => s.Id != Convert.ToInt16(model.CardId));
                        var currentTime = startTime;
                        foreach (var currentPlace in currentDayList)
                        {
                            var responseViewModel = RegulizeDayCalendar(model.ItineraryBoardCopyData, lastPlace, currentPlace, currentTime, endTimeOfDay);
                            if (!responseViewModel.Success)
                            {
                                return responseViewModel;
                            }
                            currentTime = GetCurrnetTimeSpan(responseViewModel.CurrentPlace.EndTime);
                            currentDayByDayList.Add(currentPlace);
                            lastPlace = currentPlace;
                        }
                    }
                    else if (((index != Convert.ToInt16(model.SourceLaneId)) && (index != Convert.ToInt16(model.TargetLaneId))))
                    {
                        currentDayByDayList.AddRange(dayByDayList);
                    }
                    else if (model.IsDelete)
                    {
                        var iscardDelete = dayByDayList.Where(s => s.Id == Convert.ToInt16(model.CardId)).Any();
                        var currentDayList = dayByDayList.ToList();
                        var startTime = GetCurrnetTimeSpan(dayByDayList.FirstOrDefault().StartTime);
                        var currentSearchVenue = dayByDayList.Where(s => s.Id == Convert.ToInt16(model.CardId)).FirstOrDefault();
                        if (iscardDelete)
                        {
                            currentDayList.RemoveAt(dayByDayList.FindIndex(s => s.Id == Convert.ToInt16(model.CardId)));
                        }
                        currentDayList.FirstOrDefault().StartTime = startTime.ToString();
                        currentDayList.FirstOrDefault().EndTime = startTime.Add(new TimeSpan(2, 0, 0)).ToString();
                        if (lastPlace.Id == 0)
                        {
                            lastPlace = currentDayList.FirstOrDefault();
                        }
                        currentDayByDayList.Add(currentDayList.FirstOrDefault());
                        var currentTime = startTime.Add(new TimeSpan(2, 0, 0));
                        foreach (var currentPlace in currentDayList.Skip(1))
                        {
                            var responseViewModel = RegulizeDayCalendar(model.ItineraryBoardCopyData, lastPlace, currentPlace, currentTime, endTimeOfDay);
                            if (!responseViewModel.Success)
                            {
                                return responseViewModel;
                            }
                            currentTime = GetCurrnetTimeSpan(responseViewModel.CurrentPlace.EndTime);
                            currentDayByDayList.Add(currentPlace);
                            lastPlace = currentPlace;
                        }
                    }
                    searchVenueList.Add(currentDayByDayList);
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return new ItineraryBoardResponseViewModel
                {
                    Success = false,
                    IsSourceCountZero = false,
                    IsValidDandD = false,
                    ItineraryBoardData = searchModel
                };
            }
            return new ItineraryBoardResponseViewModel
            {
                Success = true,
                IsSourceCountZero = false,
                IsTargetDateExceed = false,
                IsValidDandD = true,
                ItineraryBoardData = searchVenueList
            };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/getVenueDayByDay/all")]
        public async Task<List<List<SearchVenue>>> GetItineraryPlaces([FromBody] GetSavedEventsDaybyDayDataViewModel model)
        {
            var configurationsetting = "4,2,10,20,2,2";
            var configurationsettingArray = configurationsetting.Split(",");
            var slowPacedDefaultTime = configurationsettingArray[0];
            var fastPacedDefaultTime = configurationsettingArray[1];
            var startTimeOfDay = configurationsettingArray[2];
            var endTimeOfDay = configurationsettingArray[3];
            var spendTimeForShop = configurationsettingArray[4];
            var spendTimeForEat = configurationsettingArray[5];
            var oneDayDurationTime = 1500;
            var sortOrder = 1;
            var searchVenueList = new List<List<SearchVenue>>();

            var veneueResultFromQuerySender = await _querySender.Send(new SearchVenueQuery
            {
                CityName = model.QueryString,
                CityIds = model.CityIds,
                Categories = model.Categories,
                Speed = model.Speed,
                SlowPacedDefaultTime = slowPacedDefaultTime,
                FastPacedDefaultTime = fastPacedDefaultTime,
                SpendTimeForShop = spendTimeForShop,
                SpendTimeForEat = spendTimeForEat,
                BudgetRange = model.BudgetRange
            });

            if (veneueResultFromQuerySender.Venues.Count() == 0)
            {
                return searchVenueList;
            }
            try
            {
                veneueResultFromQuerySender.Venues = veneueResultFromQuerySender.Venues.Where(s => s.CountryName == veneueResultFromQuerySender.Venues.FirstOrDefault().CountryName).ToList();
                foreach (var place in veneueResultFromQuerySender.Venues)
                {
                    try
                    {
                        var googleService = await _latLongProvider.GetLatLong((place.EventSource == EventSource.None ? place.EventName : place.Address) + " " + place.CityName + " " + place.CountryName);
                        if (googleService != null && googleService.lat != null && googleService.lng != null)
                        {
                            place.Latitude = googleService.lat.ToString().Split("E")[0];
                            place.Longitude = googleService.lng.ToString().Split("E")[0];
                            place.Address = googleService.FullAddress;
                        }
                    }
                    catch (Exception e) { }
                }
                veneueResultFromQuerySender.Venues = veneueResultFromQuerySender.Venues.Where(s => (s.Latitude != null || s.Longitude != null)).ToList();
                List<string> Citites = model.QueryString.Split(',').ToList();
                var allPlaces = veneueResultFromQuerySender.Venues.OrderBy(s => Citites.IndexOf(s.CityName)).ToList();
                var allUpdatedPlace = new List<SearchVenue>();
                foreach (var serchVenue in allPlaces)
                {
                    var currentVenue = _placePriceProvider.GetPlacePrice(serchVenue, serchVenue.EventDetails, serchVenue.EventTicketDetails, serchVenue.ticketCategories, serchVenue.EventTicketAttributes);
                    if (currentVenue.AdultETAId != 0 && currentVenue.ChildETAId != 0)
                    {
                        allUpdatedPlace.Add(currentVenue);
                    }
                }
                allPlaces = allUpdatedPlace;
                double totalDays = (model.EndDate - model.StartDate).TotalDays;
                totalDays = totalDays + 1;
                var totalHoursToAvail = totalDays * 10;
                var totalMins = totalHoursToAvail * 60;
                var startingPoint = allPlaces.FirstOrDefault(); // create starting point as first place of the first select city
                var lastPlace = startingPoint;
                for (DateTime currentDate = model.StartDate; currentDate <= model.EndDate;)
                {
                    List<SearchVenue> DayList = new List<SearchVenue>();
                    if (allPlaces.Count() == 0)
                    {
                        break;
                    }
                    TimeSpan startTime = TimeSpan.Parse(startTimeOfDay + ":00");
                    var coord = new System.Device.Location.GeoCoordinate(Double.Parse(startingPoint.Latitude), Double.Parse(startingPoint.Longitude));
                    for (var currentTime = startTime; currentTime <= TimeSpan.Parse(endTimeOfDay + ":00");)
                    {
                        if (allPlaces.Count() == 0)
                        {
                            break;
                        }
                        if (currentTime == startTime)
                        {
                            if (currentDate == model.StartDate)
                            {
                                var googleService = await _latLongProvider.GetLatLong(startingPoint.Address);
                                startingPoint = _nearestPlaceProvider.GetNearestPlace(allPlaces, coord);
                            }
                            else
                            {
                                coord = new System.Device.Location.GeoCoordinate(Double.Parse(lastPlace.Latitude), Double.Parse(lastPlace.Longitude));
                                startingPoint = _nearestPlaceProvider.GetNearestPlace(allPlaces, coord);
                                var result = _visitDurationProvider.GetDistance((lastPlace.Address.Replace('#', ' ')), (startingPoint.Address).Replace('#', ' '));
                                var durationValue = "";
                                try
                                {
                                    durationValue = GetDurationTime(result);
                                }
                                catch (Exception e)
                                {
                                    durationValue = "2 hours";
                                }
                                startingPoint.TravelDuration = durationValue;
                            }
                            startingPoint.StartTime = currentTime.ToString();
                            startingPoint.EndTime = currentTime.Add(new TimeSpan(2, 0, 0)).ToString();
                            startingPoint.PlaceVisitDate = currentDate.ToString("yyyy-MM-dd");
                            startingPoint.SortOrder = sortOrder;
                            currentTime = currentTime.Add(new TimeSpan(2, 0, 0));
                            if (currentTime.Add(new TimeSpan(2, 0, 0)) > TimeSpan.Parse(endTimeOfDay + ":00"))
                            {
                                startTimeOfDay = "10";
                                break;
                            }
                            DayList.Add(startingPoint);
                            lastPlace = startingPoint;
                            sortOrder += 1;
                            var indexToRemove = allPlaces.FindIndex(s => s.Id == startingPoint.Id);
                            allPlaces.RemoveAt(indexToRemove);
                        }
                        else
                        {
                            var foundPlace = _nearestPlaceProvider.GetNearestPlace(allPlaces, coord);
                            var result = _visitDurationProvider.GetDistance((lastPlace.Address.Replace('#', ' ')), (foundPlace.Address).Replace('#', ' '));
                            /* if (!result.rows.Any())
                             {
                                 result = _visitDurationProvider.GetDistance((lastPlace.Name + ", " + lastPlace.CityName + ", " + lastPlace.CountryName), foundPlace.Name + ", " + foundPlace.CityName + ", " + foundPlace.CountryName);
                                 if (!result.rows.Any())
                                 {
                                     continue;
                                 }
                             } */
                            //var durationTime = _durationTimeProvider.DurationTime(result);
                            var durationTime = new DurationModel
                            {
                                DurationTime = 120,
                                DurationValue = "2 hours"
                            };
                            var currentDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, (int)currentTime.Hours, currentTime.Minutes, currentTime.Seconds);
                            currentTime = currentTime.Add(new TimeSpan(0, durationTime.DurationTime + 30, 0));
                            var totalMinutes = (int)(currentTime + new TimeSpan(0, 30, 0)).TotalMinutes;
                            currentTime = new TimeSpan(0, totalMinutes - totalMinutes % 30, 0);
                            var journeyDateTime = currentDateTime.AddMinutes(durationTime.DurationTime + 30); // Add Timespan of the total journey
                            if (currentDateTime.Date != journeyDateTime.Date)
                            {
                                startTimeOfDay = (journeyDateTime.Hour + 2).ToString();
                                break;
                            }
                            else
                            {
                                startTimeOfDay = "10";
                            }
                            if (currentTime.Add(new TimeSpan(2, 0, 0)) > TimeSpan.Parse(endTimeOfDay + ":00"))
                            {
                                break;
                            }
                            foundPlace.StartTime = currentTime.ToString();
                            foundPlace.EndTime = currentTime.Add(new TimeSpan(2, 0, 0)).ToString();
                            foundPlace.PlaceVisitDate = currentDate.ToString("yyyy-MM-dd");
                            foundPlace.PlaceVisitDuration = durationTime.DurationValue;
                            foundPlace.TravelDuration = durationTime.DurationValue;
                            startingPoint.SortOrder = sortOrder;
                            sortOrder += 1;
                            currentTime = currentTime.Add(new TimeSpan(2, 0, 0));
                            lastPlace = foundPlace;
                            DayList.Add(foundPlace);
                            var indexToRemove = allPlaces.FindIndex(s => s.Id == foundPlace.Id);
                            allPlaces.RemoveAt(indexToRemove);
                        }
                    }
                    searchVenueList.Add(DayList);
                    currentDate = currentDate.AddDays(1);
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return searchVenueList;
            }
            try
            {
                searchVenueList = searchVenueList.Where(s => s.Count > 0).ToList();
                // geo currency updates
                foreach (List<SearchVenue> searchVenues in searchVenueList)
                {
                    foreach (SearchVenue searchvenue in searchVenues)
                    {
                        _geoCurrency.SearchVenueUpdate(searchvenue, HttpContext);
                        searchvenue.AdultPrice = Decimal.Parse(searchvenue.AdultPrice.ToString("0.00"));
                        searchvenue.ChildPrice = Decimal.Parse(searchvenue.ChildPrice.ToString("0.00"));
                        searchvenue.Price = Decimal.Parse(searchvenue.Price.ToString("0.00"));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
            }
            return searchVenueList;
        }

        private int getDurationInMins(TravelSpeed SpeedType, string fastPacedDefaultTime, string slowPacedDefaultTime, int popularPacedDefaultTime)

        {
            int minsToReturm;
            if (SpeedType == TravelSpeed.Fast)
                minsToReturm = int.Parse(fastPacedDefaultTime) * 60;
            else if (SpeedType == TravelSpeed.Slow)
                minsToReturm = int.Parse(slowPacedDefaultTime) * 60;
            else
                minsToReturm = popularPacedDefaultTime * 60;

            return minsToReturm;
        }

        [HttpGet]
        [Route("api/latlongutility/{cityName}")]
        public async Task<string> SaveUtility(String cityName)
        {
            try
            {
                var veneueResult = await _querySender.Send(new GoogleAPIUtilityQuery
                {
                    CityName = cityName,
                    IsAll = false
                });

                foreach (FIL.Contracts.Models.GoogleAPIUtility currentVenue in veneueResult.Venues)
                {
                    var address = currentVenue.Venue + ", " + currentVenue.City; // get by address and city
                    HttpWebRequest request = WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=AIzaSyCMGKGiUA__vE3FFhqZBEHZiFV78fDbeiI") as HttpWebRequest;

                    //request.Accept = "application/xrds+xml";  
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    WebHeaderCollection header = response.Headers;

                    var encoding = ASCIIEncoding.ASCII;
                    RootObject result = new RootObject();
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        string responseText = reader.ReadToEnd();
                        result = JsonConvert.DeserializeObject<RootObject>(responseText);
                    }

                    if (result.results.Count() > 0) // if API return data
                    {
                        var lat = result.results[0].geometry.location.lat.ToString();
                        var longitude = result.results[0].geometry.location.lng.ToString();
                        GoogleAPIUtilityCommandResult placeCalendarResult = await _commandSender.Send<GoogleAPIUtilityCommand, GoogleAPIUtilityCommandResult>(new GoogleAPIUtilityCommand
                        {
                            VenueId = currentVenue.VenueId,
                            Latitude = lat,
                            Longitude = longitude
                        });
                    }
                    else // if API doesn't return by address and city then excute else
                    {
                        address = currentVenue.City;
                        request = WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=AIzaSyCMGKGiUA__vE3FFhqZBEHZiFV78fDbeiI") as HttpWebRequest;

                        //request.Accept = "application/xrds+xml";  
                        response = (HttpWebResponse)request.GetResponse();

                        header = response.Headers;

                        encoding = ASCIIEncoding.ASCII;
                        result = new RootObject();
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            string responseText = reader.ReadToEnd();
                            result = JsonConvert.DeserializeObject<RootObject>(responseText);
                        }
                        if (result.results.Count() > 0)
                        {
                            var lat = result.results[0].geometry.location.lat.ToString();
                            var longitude = result.results[0].geometry.location.lng.ToString();
                            GoogleAPIUtilityCommandResult placeCalendarResult = await _commandSender.Send<GoogleAPIUtilityCommand, GoogleAPIUtilityCommandResult>(new GoogleAPIUtilityCommand
                            {
                                VenueId = currentVenue.VenueId,
                                Latitude = lat,
                                Longitude = longitude
                            });
                        }
                    }
                }
                return "Request succesful";
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return "Request unsuccessful";
            }

        }

        [HttpGet]
        [Route("api/save/latlongutility")]
        public async Task<string> SaveAllUtility()
        {
            try
            {
                var veneueResult = await _querySender.Send(new GoogleAPIUtilityQuery
                {
                    IsAll = true
                });

                foreach (FIL.Contracts.Models.GoogleAPIUtility currentVenue in veneueResult.Venues)
                {
                    var address = currentVenue.Venue + ", " + currentVenue.City; // get by address and city
                    HttpWebRequest request = WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=AIzaSyCMGKGiUA__vE3FFhqZBEHZiFV78fDbeiI") as HttpWebRequest;

                    //request.Accept = "application/xrds+xml";  
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    WebHeaderCollection header = response.Headers;

                    var encoding = ASCIIEncoding.ASCII;
                    RootObject result = new RootObject();
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        string responseText = reader.ReadToEnd();
                        result = JsonConvert.DeserializeObject<RootObject>(responseText);
                    }

                    if (result.results.Count() > 0) // if API return data
                    {
                        var lat = result.results[0].geometry.location.lat.ToString();
                        var longitude = result.results[0].geometry.location.lng.ToString();
                        GoogleAPIUtilityCommandResult placeCalendarResult = await _commandSender.Send<GoogleAPIUtilityCommand, GoogleAPIUtilityCommandResult>(new GoogleAPIUtilityCommand
                        {
                            VenueId = currentVenue.VenueId,
                            Latitude = lat,
                            Longitude = longitude
                        });
                    }
                    else // if API doesn't return by address and city then excute else
                    {
                        address = currentVenue.City;
                        request = WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=AIzaSyCMGKGiUA__vE3FFhqZBEHZiFV78fDbeiI") as HttpWebRequest;

                        //request.Accept = "application/xrds+xml";  
                        response = (HttpWebResponse)request.GetResponse();

                        header = response.Headers;

                        encoding = ASCIIEncoding.ASCII;
                        result = new RootObject();
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            string responseText = reader.ReadToEnd();
                            result = JsonConvert.DeserializeObject<RootObject>(responseText);
                        }
                        if (result.results.Count() > 0)
                        {
                            var lat = result.results[0].geometry.location.lat.ToString();
                            var longitude = result.results[0].geometry.location.lng.ToString();
                            GoogleAPIUtilityCommandResult placeCalendarResult = await _commandSender.Send<GoogleAPIUtilityCommand, GoogleAPIUtilityCommandResult>(new GoogleAPIUtilityCommand
                            {
                                VenueId = currentVenue.VenueId,
                                Latitude = lat,
                                Longitude = longitude
                            });
                        }
                    }
                }
                return "Request succesful";
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return "Request unsuccessful";
            }

        }

        [HttpGet]
        [Route("api/Itinerary/ItineraryTicketData")]
        public async Task<ItineraryTicketQueryResult> ItineraryTicketData(List<Int64> eventIds)
        {
            ItineraryTicketQueryResult itineraryTicketQueryResult = await _querySender.Send(new ItineraryTicketQuery
            {
                eventIds = eventIds
            });

            //geo currency updates
            foreach (ItineraryTicketDetails itineraryTicketDetails in itineraryTicketQueryResult.itineraryTicketDetails)
            {
                foreach (Contracts.Models.EventTicketAttribute eventTicketAttribute in itineraryTicketDetails.eventTicketAttributes)
                {
                    _geoCurrency.eventTicketAttributeUpdate(eventTicketAttribute, HttpContext);
                }
            }

            return itineraryTicketQueryResult;
        }



        public class Distance
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Duration
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Element
        {
            public Distance distance { get; set; }
            public Duration duration { get; set; }
            public string status { get; set; }
        }

        public class Row
        {
            public Element[] elements { get; set; }
        }
        public class Parent
        {
            public string[] destination_addresses { get; set; }
            public string[] origin_addresses { get; set; }
            public Row[] rows { get; set; }
            public string status { get; set; }
        }

        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }

        public class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Bounds
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Northeast2
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest2
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Viewport
        {
            public Northeast2 northeast { get; set; }
            public Southwest2 southwest { get; set; }
        }

        public class Geometry
        {
            public Bounds bounds { get; set; }
            public Location location { get; set; }
            public string location_type { get; set; }
            public Viewport viewport { get; set; }
        }

        public class Result
        {
            public List<AddressComponent> address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public List<string> types { get; set; }
        }

        public class RootObject
        {
            public List<Result> results { get; set; }
            public string status { get; set; }
        }
    }
}