using System;
using System.Threading.Tasks;
using FIL.Web.Kitms.Feel.ViewModels.EventCreation;
using Microsoft.AspNetCore.Mvc;
using FIL.Contracts.Commands.EventCreation;
using Microsoft.AspNetCore.Authorization;
using FIL.Foundation.Senders;
using System.Collections.Generic;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.EventCreation;
using FIL.Contracts.Queries.VenueCreation;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Queries.Country;
using FIL.Contracts.Commands.Location;
using FIL.Web.Kitms.Feel.ViewModels.Finance;
using FIL.Web.Kitms.Feel.ViewModels.Tag;
using FIL.Contracts.Commands.FinanceDetails;
using FIL.Web.Core;
using System.Drawing;
using System.IO;
using FIL.Web.Core.Providers;
using Microsoft.AspNetCore.Http;
using System.Linq;
using FIL.Web.Kitms.Feel.ViewModels.Event;
using FIL.Contracts.Commands.EventCategoryMapping;
using FIL.Contracts.Commands.EventSiteIdMapping;
using FIL.Contracts.Queries.FinanceDetail;
using FIL.Contracts.Queries.Tags;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using FIL.Contracts.DataModels;
using FIL.Web.Kitms.Feel.ViewModels.CustomerIpDetails;
using FIL.Web.Kitms.Feel.ViewModels.Search;
using FIL.Contracts.QueryResults;
using FIL.Contracts.Queries.Venue;
using System.Net;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using FIL.Configuration;
using MoreLinq;
using System.Device.Location;

namespace FIL.Web.Kitms.Feel.Controllers
{
  public class EventCreationController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly IAmazonS3FileProvider _amazonS3FileProvider;
    private readonly IClientIpProvider _clientIpProvider;
    private object eventCategories;
    private readonly ISessionProvider _sessionProvider;
    private IHostingEnvironment _env;
    private readonly ISettings _settings;

    public EventCreationController(ICommandSender commandSender, IQuerySender querySender,
        IAmazonS3FileProvider amazonS3FileProvider, IClientIpProvider clientIpProvider,
        ISessionProvider sessionProvider, IHostingEnvironment env, ISettings settings)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _amazonS3FileProvider = amazonS3FileProvider;
      _clientIpProvider = clientIpProvider;
      _sessionProvider = sessionProvider;
      _env = env;
      _settings = settings;
    }

    [HttpGet]
    [Route("api/finance/{financeModel}")]
    public async Task<FinancDetailCommand> GetFinanceByEventId(Guid financeModel)
    {
      var queryResult = await _querySender.Send(new FinancDetailsByIdQuery { EventId = financeModel });

      var financeDat = new FinancDetailCommand();
      financeDat.Id = queryResult.Id;
      financeDat.EventId = queryResult.EventId;
      financeDat.FirstName = queryResult.FirstName;
      financeDat.LastName = queryResult.LastName;
      financeDat.FirstName = queryResult.FirstName;
      financeDat.LastName = queryResult.LastName;
      financeDat.AccountNickName = queryResult.AccountNickName;
      financeDat.BankName = queryResult.BankName;
      financeDat.Location = queryResult.location;
      financeDat.Address1 = queryResult.address1;
      financeDat.Address2 = queryResult.address2;
      financeDat.State = queryResult.state;
      financeDat.City = queryResult.city;
      financeDat.Country = queryResult.country;
      financeDat.BankAccountType = queryResult.BankAccountType;
      financeDat.BankName = queryResult.BankName;
      financeDat.PANNo = queryResult.PANNo;
      financeDat.RoutingNo = queryResult.RoutingNo;
      financeDat.GSTNo = queryResult.GSTNo;
      financeDat.AccountNo = queryResult.AccountNo;
      financeDat.AccountNickName = queryResult.AccountNickName;
      financeDat.CountryId = queryResult.CountryId;
      financeDat.StateId = queryResult.StateId;
      financeDat.CurrencyId = queryResult.CurrencyId;
      financeDat.EventDetailId = queryResult.EventDetailId;
      financeDat.FinancialsAccountBankAccountGSTInfo = queryResult.FinancialsAccountBankAccountGSTInfo;

      return financeDat;
    }

    [HttpGet]
    [Route("api/eventcategory/all")]
    public async Task<ViewModels.Event.EventCategoryDataViewModel> GetAllEventCategory()
    {
      Response.Cookies.Delete("placeAltId");
      ViewModels.Event.EventCategoryDataViewModel categoryVM = new ViewModels.Event.EventCategoryDataViewModel();
      categoryVM.categories = new List<CategoryDataViewModel>();
      var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
      {
        Id = 0
      });
      foreach (var p in eventCategoryResult.EventCategories)
      {
        if (p.IsFeel)
        {
          CategoryDataViewModel data = new CategoryDataViewModel()
          {
            CategoryId = p.EventCategoryId,
            DisplayName = p.DisplayName,
            IsFeel = p.IsFeel,
            IsHomePage = p.IsHomePage,
            Order = p.Order,
            Slug = p.Slug,
            Value = p.Id,
            MasterEventTypeId = p.MasterEventTypeId
          };
          categoryVM.categories.Add(data);
        }
      }
      return categoryVM;
    }

    [HttpGet]
    [Route("api/tags/all")]
    public async Task<TagResponseViewModel> GetAllTags()
    {
      var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Tags.TagQuery
      {

      });

      return new TagResponseViewModel
      {
        Tags = eventCategoryResult.Tags.ToList()
      };
    }

    [HttpGet]
    [Route("api/eventtype/all")]
    public EventTypeDataViewModel GetAllEventType()
    {
      List<string> eventType = new List<string>();
      foreach (EventType eventtype in Enum.GetValues(typeof(EventType)))
      {
        eventType.Add(eventtype.ToString());
      }
      return new EventTypeDataViewModel { EventType = eventType };
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("api/customeripdetail")]
    public async Task<IPDetailViewModel> GetCustomerIpDetail()
    {
      IPDetailViewModel customerIPDetail = new IPDetailViewModel();

      var customerIpDetailResult = await _querySender.Send(new Contracts.Queries.CustomerIpDetails.CustomerIpDetailQuery
      {
        Ip = _clientIpProvider.Get()
      });

      if (customerIpDetailResult != null)
      {
        return new IPDetailViewModel
        {
          Latitude = (customerIpDetailResult.IpDetail.Latitude == 0.0m || customerIpDetailResult.IpDetail.Latitude == null) ? 18.5274595m : (decimal)customerIpDetailResult.IpDetail.Latitude,
          Longitude = (customerIpDetailResult.IpDetail.Longitude == 0.0m || customerIpDetailResult.IpDetail.Latitude == null) ? 73.8293495m : (decimal)customerIpDetailResult.IpDetail.Longitude
        };
      }
      else
      {
        return new IPDetailViewModel { Latitude = 18.5274595m, Longitude = 73.8293495m };
      }
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("api/amenities/all")]
    public async Task<AmenityDataViewModel> GetAllAmenities()
    {
      List<Amenities> amenities = new List<Amenities>();

      var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.AmenitiesQuery
      {
        Id = 0
      });

      foreach (var amnty in eventCategoryResult.Amenities)
      {
        Amenities aminityData = new Amenities();
        aminityData.Id = amnty.Id;
        aminityData.Amenity = amnty.Amenity;
        amenities.Add(aminityData);
      }
      return new AmenityDataViewModel { Amenities = amenities };
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("api/getsavedevent/{id}")]
    public async Task<SaveEventDataViewModel> GetSavedEventByI(Guid id)
    {
      var result = await _querySender.Send(new SavedEventQuery()
      {
        Id = id
      });
      CookieOptions option = new CookieOptions();
      option.Expires = DateTime.Now.AddMinutes(60);
      Response.Cookies.Append("placeAltId", result.AltId.ToString(), option);
      var data = new SaveEventDataViewModel()
      {
        EventId = result.Id,
        Address1 = result.Address1,
        Address2 = result.Address2,
        AltId = result.AltId,
        AmenityId = result.AmenityId,
        Archdetail = result.Archdetail,
        ArchdetailImages = result.ArchdetailImages,
        Categoryid = result.Categoryid,
        City = result.City,
        Country = result.Country,
        DescpagebannerImage = result.DescpagebannerImage,
        Description = result.Description,
        Highlights = result.Highlights,
        History = result.History,
        Id = result.Id,
        Immersiveexperience = result.Immersiveexperience,
        InventorypagebannerImage = result.InventorypagebannerImage,
        TimelineImages = result.TimelineImages,
        TilesSliderImages = result.TilesSliderImages,
        PlacemapImages = result.PlacemapImages,
        GalleryImages = result.GalleryImages,
        Location = result.Location,
        Metadescription = result.Metadescription,
        Metatags = result.Metatags,
        Metatitle = result.Metatitle,
        PlaceName = result.Title,
        State = result.State,
        Subcategoryid = result.Subcategoryid,
        Title = result.Title,
        Zip = result.Zip,
        ArchdetailId = result.ArchdetailId,
        HistoryId = result.HistoryId,
        ImmersiveexperienceId = result.ImmersiveexperienceId,
        HighlightsId = result.HighlightsId,
        Lat = result.Lat,
        Long = result.Long,
        HourTimeDuration = result.HourTimeDuration,
        MinuteTimeDuration = result.MinuteTimeDuration,
        TagIds = result.TagIds,
        EventHostMappings = result.EventHostMappings
      };

      return data;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("api/saveevent/save")]
    public async Task<EventCreationResponseDataViewModel> SaveEvent([FromBody] SaveEventDataViewModel model)
    {
      var session = await _sessionProvider.Get();
      try
      {
        var AltId = model.AltId;
        if (AltId == Guid.Empty)
        {
          if (Request.Cookies["placeAltId"] != null)
          {
            AltId = new Guid(Request.Cookies["placeAltId"].ToString());
          }
          else
          {
            AltId = Guid.NewGuid();
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(60);
            Response.Cookies.Append("placeAltId", AltId.ToString().ToUpper(), option);
          }
        }
        var metaD = "<title>" + model.Metatitle + "</title><br/>" +
            "<meta name=\"description\" content=\"" + model.Metadescription + "\"><br/>" +
            "<meta name=\"keywords\" content=\"" + model.Metatags + "\"  >";
        if (model.Id > 0) AltId = model.AltId;
        SaveEventDataResult EventData = await _commandSender.Send<SaveEventCommand, SaveEventDataResult>(new SaveEventCommand
        {
          Id = model.Id,
          AltId = AltId,
          Name = model.Title,
          Description = model.Description,
          TermsAndConditions = "NA",
          EventCategoryId = Convert.ToInt32(model.Categoryid),
          EventType = (EventType)1,
          MetaDetails = metaD,
          IsEnabled = true,
          IsFeel = true,
          ClientPointOfContactId = 1,
          TagIds = model.TagIds,
          SubCategoryIds = model.Subcategoryid,
          AmenityIds = model.AmenityId,
          TimeDuration = model.HourTimeDuration,
          IsEdit = model.IsEdit,
          ModifiedBy = session.User.AltId
        });

        if (EventData.IsAlreadyExists)
        {
          return new EventCreationResponseDataViewModel
          {
            Success = false,
            IsAlreadyExists = true
          };
        }
        // Save Location
        SaveLocationViewModel sl = new SaveLocationViewModel();
        sl.Placename = model.PlaceName;
        sl.Location = model.Location;
        sl.State = model.State;
        sl.Title = model.Title;
        sl.Country = model.Country;
        sl.City = model.City;
        sl.Address1 = model.Address1;
        sl.Address2 = model.Address2;
        sl.TilesSliderImages = model.TilesSliderImages;
        sl.DescpagebannerImages = model.DescpagebannerImage;
        sl.InventorypagebannerImage = model.InventorypagebannerImage;
        sl.GalleryImages = model.GalleryImages;
        sl.PlacemapImages = model.PlacemapImages;
        sl.TimelineImages = model.TimelineImages;
        sl.ArchdetailImages = model.ArchdetailImages;
        sl.IsEdit = model.IsEdit;
        sl.Zip = model.Zip;
        //sl.Latitude = model.Latitude;
        //sl.Logitude = model.Logitude;
        sl.Lat = model.Lat;
        sl.Long = model.Long;
        long evntid = 0;
        if (model.Id > 0) evntid = model.Id;
        else evntid = EventData.Id;

        var response = await SavedLocation(sl, evntid);

        // Save description
        await SaveDescription(model, evntid);
        Response.Cookies.Delete("placeAltId");
        return new EventCreationResponseDataViewModel
        {
          AltId = AltId,
          EventId = evntid,
          Success = true,
          //VenueAltId = response.AltId,
          VenueAltId = Guid.NewGuid()
        };
      }
      catch (Exception ex)
      {
        new EventCreationResponseDataViewModel
        {
          Success = false,
          IsAlreadyExists = false
        };
      }
      //}

      return new EventCreationResponseDataViewModel
      {
        Success = true,
        IsAlreadyExists = false
      };
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

    [HttpPost]
    [AllowAnonymous]
    [Route("api/saveamity/save")]
    public async Task<SaveAmenityCommandResult> SaveAmenity([FromBody] SaveAmenityDataViewModel model)
    {
      //if (ModelState.IsValid)
      //{
      var session = await _sessionProvider.Get();
      try
      {

        SaveAmenityCommand command = new SaveAmenityCommand();
        command.Amenity = model.Amenity;

        return await _commandSender.Send<SaveAmenityCommand, SaveAmenityCommandResult>(command);



      }
      catch (Exception ex)
      {
        new AmenityCreationResponseDataViewModel
        {
          Success = false
        };
      }
      //}

      return new SaveAmenityCommandResult
      {
      };
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("api/getVenueDayByDay/all")]
    public async Task<List<List<SearchVenue>>> GetSavedVenue([FromBody] GetSavedEventsDaybyDayDataViewModel model)
    {
      // var configSetting = _settings.GetConfigSetting("ItineraryAlgoSettings");
      //var configurationsetting = configSetting.Value.ToString();
      var configurationsetting = "4,2,10,8,2,2";
      var configurationsettingArray = configurationsetting.Split(",");
      var slowPacedDefaultTime = configurationsettingArray[0];
      var fastPacedDefaultTime = configurationsettingArray[1];
      var startTimeOfDay = configurationsettingArray[2];
      var endTimeOfDay = configurationsettingArray[3];
      var spendTimeForShop = configurationsettingArray[4];
      var spendTimeForEat = configurationsettingArray[5];
      var popularPacedDefaultTime = 2;
      var totalMinutesInFourHrs = 240;
      var oneDayDurationTime = 1500;

      DateTime initialTime = new DateTime(2008, 01, 02, int.Parse(startTimeOfDay), 00, 00);
      DateTime lastTime = new DateTime(2008, 01, 02, int.Parse(endTimeOfDay) + 12, 00, 00);

      TimeSpan duration = lastTime - initialTime;
      var totalminutesToVisitPerDay = duration.TotalMinutes - 50;
      var veneueResultFromQuerySender = await _querySender.Send(new SearchVenueQuery
      {
        CityName = model.QueryString,
        Categories = model.Categories,
        Speed = model.Speed,
        SlowPacedDefaultTime = slowPacedDefaultTime,
        FastPacedDefaultTime = fastPacedDefaultTime,
        SpendTimeForShop = spendTimeForShop,
        SpendTimeForEat = spendTimeForEat,
        BudgetRange = model.BudgetRange
      });

      var groupedVal = veneueResultFromQuerySender.Venues.GroupBy(x => x.EventId);

      //var veneueResult = veneueResultFromQuerySender.Venues.DistinctBy(x => x.EventId).ToList<SearchVenue>();

      var veneueResult = new List<SearchVenue>(); ;
      foreach (var groupedValItem in groupedVal)
      {
        var groupedValItemList = groupedValItem.ToList();
        if (groupedValItemList.Count() == 1)
        {
          veneueResult.Add(groupedValItemList[0]);
        }
        else
        {
          var groupedValItemListDistinct = groupedValItemList.DistinctBy(x => x.CategoryName).ToList();
          if (groupedValItemListDistinct.Count() == 1)
          {
            veneueResult.Add(groupedValItemListDistinct[0]);
          }
          else
          {
            var categeroryName = "";
            foreach (var groupedValItemListDistinctItem in groupedValItemListDistinct)
            {
              categeroryName = categeroryName + groupedValItemListDistinctItem.CategoryName + ",";
            }
            categeroryName = categeroryName.TrimEnd(',');
            SearchVenue searchVenue = new SearchVenue();
            searchVenue.CategoryName = categeroryName;
            searchVenue.CityId = groupedValItemListDistinct[0].CityId;
            searchVenue.Currency = groupedValItemListDistinct[0].Currency;
            searchVenue.EventName = groupedValItemListDistinct[0].Name;
            searchVenue.EventAltId = groupedValItemListDistinct[0].EventAltId;
            searchVenue.EventDescription = groupedValItemListDistinct[0].EventDescription;
            searchVenue.EventSlug = groupedValItemListDistinct[0].EventSlug;
            searchVenue.PlaceVisitDuration = groupedValItemListDistinct[0].PlaceVisitDuration;
            searchVenue.Latitude = groupedValItemListDistinct[0].Latitude;
            searchVenue.Longitude = groupedValItemListDistinct[0].Longitude;
            searchVenue.Image = groupedValItemListDistinct[0].Image;
            searchVenue.EventId = groupedValItemListDistinct[0].EventId;

            veneueResult.Add(searchVenue);
          }
        }
      }

      //var veneueResult = veneueResultFromQuerySender.Venues.DistinctBy(x => x.EventId).ToList<SearchVenue>();


      List<SearchVenue> allPlaces = new List<SearchVenue>();
      List<List<SearchVenue>> placesToAdd = new List<List<SearchVenue>>();

      foreach (var item in veneueResult)
      {
        allPlaces.Add(item);
      }
      if (allPlaces.Count() > 0)
      {
        double totalDays = (model.EndDate - model.StartDate).TotalDays;
        totalDays = totalDays + 1;
        var totalHoursToAvail = totalDays * 10;
        var totalMins = totalHoursToAvail * 60;

        var startingPoint = allPlaces[0];
        var totalCalculatedMins = 0;

        TimeSpan startTime = TimeSpan.Parse(startTimeOfDay + ":00");
        List<SearchVenue> daybydaylist = new List<SearchVenue>();
        int placeCount = 0;
        for (int count = 0; count < totalMins;)
        {
          if (startingPoint.Latitude != null)
          {
            var coord = new System.Device.Location.GeoCoordinate(Double.Parse(startingPoint.Latitude), Double.Parse(startingPoint.Longitude));
            placeCount++;
            if (allPlaces.Count > 0)
            {
              List<SearchVenue> allPlacesToContinue = new List<SearchVenue>();
              foreach (var item in allPlaces)
              {
                allPlacesToContinue.Add(item);
              }
              var itemsToRemove = allPlaces.Where(r => r.Latitude == null);
              foreach (var itemToRemove in itemsToRemove)
              {
                allPlacesToContinue.Remove(itemToRemove);
              }

              allPlaces = allPlacesToContinue;

              var nearest = allPlaces.Select(x => new System.Device.Location.GeoCoordinate(Double.Parse(x.Latitude), Double.Parse(x.Longitude)))
                                     .OrderBy(x => x.GetDistanceTo(coord))
                                     .First();
              string requestUri = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + startingPoint.Latitude + "," + startingPoint.Longitude + "&destinations=" + nearest.Latitude + "," + nearest.Longitude + "&mode=driving&language=en-EN&sensor=false&key=AIzaSyC_d6zsggzpRgheamvCauiFsK750QNwaA0");


              HttpWebRequest request = WebRequest.Create("https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + Double.Parse(startingPoint.Latitude) + "," + Double.Parse(startingPoint.Longitude) + "&destinations=" + nearest.Latitude + "," + nearest.Longitude + "&mode=driving&language=en-EN&sensor=false&key=AIzaSyC_d6zsggzpRgheamvCauiFsK750QNwaA0") as HttpWebRequest;

              //getDistance(startingPoint, nearest);
              //request.Accept = "application/xrds+xml";  
              HttpWebResponse response = (HttpWebResponse)request.GetResponse();

              WebHeaderCollection header = response.Headers;

              var encoding = ASCIIEncoding.ASCII;
              Parent result = new Parent();
              using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
              {
                string responseText = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<Parent>(responseText);
              }
              String durationValueStatus = "OK";
              String durationValue;
              if (result.rows[0].elements[0].status == "ZERO_RESULTS")
              {
                durationValueStatus = "ZERO_RESULTS";
                durationValue = "0 min";
              }
              else
              {
                durationValueStatus = "OK";
                durationValue = result.rows[0].elements[0].duration.text;
              }


              var durationTime = 0;
              try
              {
                if (durationValue.Split("days").Count() > 1)
                {
                  durationTime = ((int.Parse(durationValue.Split("days")[0]) * 60) * 24) + (int.Parse(durationValue.Split("days")[1].Split("hours")[0]) * 60);
                }
                if (durationValue.Split("day").Count() > 1)
                {
                  durationTime = ((int.Parse(durationValue.Split("day")[0]) * 60) * 24) + (int.Parse(durationValue.Split("day")[1].Split("hours")[0]) * 60);
                }
                else if (durationValue.Split("hours").Count() > 1)
                {
                  durationTime = (int.Parse(durationValue.Split("hours")[0]) * 60) + int.Parse(durationValue.Split("hours")[1].Split("mins")[0]);
                }

                else if (durationValue.Split("hour").Count() > 1)
                {
                  durationTime = (int.Parse(durationValue.Split("hour")[0]) * 60) + int.Parse(durationValue.Split("hour")[1].Split("mins")[0]);
                }

                else
                {
                  durationTime = int.Parse(Regex.Replace(durationValue.ToString(), "[^0-9]+", string.Empty));

                  //if (int.Parse(durationTime) > 60)
                  //{
                  //    int timePart2;
                  //    int timePart1 = int.Parse(durationValue.Split("hour")[0]) * 60;


                  //        timePart2 = int.Parse(durationValue.Split("hour")[1].Split("mins")[0]);

                  //    durationTime = (int.ParsetimePart1 + timePart2).ToString();

                  //}
                }
              }
              catch (Exception ex)
              {
                durationTime = oneDayDurationTime;
              }

              int totalDurationMinutes = durationTime;
              var timeDuration1 = totalDurationMinutes / 60;
              var timeDuration2 = totalDurationMinutes % 60;
              var secondDurationTime = string.Format("{0:00}:{1:00}", timeDuration1, timeDuration2);
              TimeSpan tDuration2;
              try { tDuration2 = TimeSpan.Parse(secondDurationTime); }
              catch (Exception ex) { tDuration2 = TimeSpan.Parse("23:00"); };


              var foundPlace = allPlaces.Find(x => Double.Parse(x.Latitude) == nearest.Latitude);
              var entryIndex = allPlaces.FindIndex(x => x.Id == foundPlace.Id);
              var searchDuration = durationValue;
              if (totalCalculatedMins == 0)
                searchDuration = "0";



              if (durationValueStatus != "ZERO_RESULTS" && totalDurationMinutes < totalMinutesInFourHrs && totalCalculatedMins < totalminutesToVisitPerDay)
              {
                if (allPlaces[entryIndex].PlaceVisitDuration == null)
                  allPlaces[entryIndex].PlaceVisitDuration = "2";

                var totalDurationTime = allPlaces[entryIndex].PlaceVisitDuration.Split(":");
                int totalDurationTimeMinutes;
                if (totalDurationTime.Count() == 1)
                {
                  try
                  { totalDurationTimeMinutes = (int.Parse(totalDurationTime[0])) * 60; }
                  catch (Exception ex)
                  {
                    totalDurationTimeMinutes = getDurationInMins(allPlaces[entryIndex].Speed, fastPacedDefaultTime, slowPacedDefaultTime, popularPacedDefaultTime);
                  }
                }
                else if (totalDurationTime[0] == "")
                {
                  try { totalDurationTimeMinutes = (int.Parse(totalDurationTime[1])); }
                  catch (Exception ex)
                  {
                    totalDurationTimeMinutes = getDurationInMins(allPlaces[entryIndex].Speed, fastPacedDefaultTime, slowPacedDefaultTime, popularPacedDefaultTime);

                  }
                }
                else
                  try { totalDurationTimeMinutes = (int.Parse(totalDurationTime[0]) * 60) + (int.Parse(totalDurationTime[1])); }
                  catch (Exception ex)
                  {
                    totalDurationTimeMinutes = getDurationInMins(allPlaces[entryIndex].Speed, fastPacedDefaultTime, slowPacedDefaultTime, popularPacedDefaultTime);

                  }


                var time1 = totalDurationTimeMinutes / 60;
                var time2 = totalDurationTimeMinutes % 60;
                var secondTime = string.Format("{0:00}:{1:00}", time1, time2);
                TimeSpan t2 = TimeSpan.Parse(secondTime);

                TimeSpan endTime = startTime.Add(t2);
                SearchVenue searchVenue = new SearchVenue();


                searchVenue.CurrencyExchangeRate = allPlaces[entryIndex].CurrencyExchangeRate;
                searchVenue.PlaceVisitDuration = allPlaces[entryIndex].PlaceVisitDuration;
                searchVenue.CityId = allPlaces[entryIndex].CityId;
                searchVenue.CategoryName = allPlaces[entryIndex].CategoryName;
                searchVenue.EventName = allPlaces[entryIndex].EventName;
                searchVenue.EventDescription = allPlaces[entryIndex].EventDescription;
                searchVenue.EventSlug = allPlaces[entryIndex].EventSlug;
                searchVenue.Latitude = allPlaces[entryIndex].Latitude;
                searchVenue.Longitude = allPlaces[entryIndex].Longitude;
                searchVenue.CityName = allPlaces[entryIndex].CityName;
                searchVenue.Currency = allPlaces[entryIndex].Currency;
                searchVenue.EventAltId = allPlaces[entryIndex].EventAltId;
                searchVenue.EventId = allPlaces[entryIndex].EventId;

                var startTimeToAdd = startTime.Add(tDuration2).ToString();

                string startTimeToShow;
                var splitStartTime = startTimeToAdd.Split(":");
                int secondOne = int.Parse(splitStartTime[1]);

                if (totalCalculatedMins == 0)
                {
                  string secondOneToInput = "00";
                  startTimeToShow = splitStartTime[0] + ":" + secondOneToInput + ":" + splitStartTime[2];
                }
                else
                {
                  if (secondOne > 0 && secondOne < 30)
                  {
                    string secondOneToInput = "30";
                    startTimeToShow = splitStartTime[0] + ":" + secondOneToInput + ":" + splitStartTime[2];
                  }
                  else if (secondOne > 30 && secondOne <= 59)
                  {
                    string secondOneToInput = "00";
                    string firstOneToInput = (int.Parse(splitStartTime[0]) + 1).ToString();
                    startTimeToShow = firstOneToInput + ":" + secondOneToInput + ":" + splitStartTime[2];
                  }
                  else
                  {
                    startTimeToShow = startTime.Add(tDuration2).ToString();
                  }
                }


                var endTimeToAdd = endTime.Add(tDuration2).ToString();

                string endTimeToShow;


                var splitendTime = endTimeToAdd.Split(":");
                int secondOneLast = int.Parse(splitendTime[1]);

                if (totalCalculatedMins == 0)
                {
                  string secondOneToInput = "00";
                  endTimeToShow = splitendTime[0] + ":" + secondOneToInput + ":" + splitendTime[2];
                }
                else
                {
                  if (secondOneLast > 0 && secondOneLast <= 30)
                  {
                    string secondOneToInput = "30";
                    endTimeToShow = splitendTime[0] + ":" + secondOneToInput + ":" + splitendTime[2];
                  }
                  else if (secondOneLast > 30 && secondOneLast <= 59)
                  {
                    string secondOneToInput = "00";
                    string firstOneToInput = (int.Parse(splitendTime[0].Split(".")[0]) + 1).ToString();
                    endTimeToShow = firstOneToInput + ":" + secondOneToInput + ":" + splitendTime[2];
                  }
                  else
                  {
                    endTimeToShow = endTime.Add(tDuration2).ToString();
                  }
                }

                searchVenue.StartTime = startTimeToShow;
                searchVenue.EndTime = endTimeToShow;
                searchVenue.Image = allPlaces[entryIndex].Image;

                searchVenue.TravelDuration = searchDuration;

                searchVenue.Price = allPlaces[entryIndex].Price;

                searchVenue.PriceInDollar = allPlaces[entryIndex].PriceInDollar;

                endTime = endTime.Add(tDuration2);
                startTime = TimeSpan.Parse(endTimeToShow);
                searchVenue.PlaceVisitDate = model.StartDate.Date.ToString("yyyy-MM-dd");
                #region Check Place off day
                if (allPlaces[entryIndex].PlaceOffDaysList != null && allPlaces[entryIndex].PlaceOffDaysList.Count > 0)
                {
                  if (allPlaces[entryIndex].PlaceOffDaysList.Where
                      (x => x.WeekOffDay.ToString() == DateTime.Parse(searchVenue.PlaceVisitDate).DayOfWeek.ToString()).Count() > 0
                      ||
                      allPlaces[entryIndex].PlaceOffDaysList.Where
                      (x => x.WeekOffDay.ToString() == "All").Count() > 0
                      )

                  {

                    if (allPlaces[entryIndex].PlaceHolidaysList.Where
                        (x => x.LeaveDateTime.ToString() == DateTime.Parse(searchVenue.PlaceVisitDate).Date.ToString()).Count() == 0)

                    {
                      #region Check budget limit with refernce to adults number
                      if (model.BudgetRange == BudgetRange.First)
                      {
                        if ((daybydaylist.Sum(x => x.PriceInDollar)) * model.NoOfAdults < 1000)
                        {
                          daybydaylist.Add(searchVenue);
                        }
                        else
                          break;
                      }
                      if (model.BudgetRange == BudgetRange.Second)
                      {
                        if ((daybydaylist.Sum(x => x.PriceInDollar)) * model.NoOfAdults < 3000)
                        {
                          daybydaylist.Add(searchVenue);
                        }
                        else
                          break;
                      }
                      if (model.BudgetRange == BudgetRange.Third)
                      {
                        if ((daybydaylist.Sum(x => x.PriceInDollar)) * model.NoOfAdults < 5000)
                        {
                          daybydaylist.Add(searchVenue);
                        }
                        else
                          break;
                      }
                      if (model.BudgetRange == BudgetRange.Fourth)
                      {
                        daybydaylist.Add(searchVenue);
                      }
                      #endregion

                    }

                  }
                }
                else
                {

                  #region Check Place leave day
                  if (allPlaces[entryIndex].PlaceHolidaysList != null && allPlaces[entryIndex].PlaceHolidaysList.Where
                     (x => x.LeaveDateTime.ToString() == DateTime.Parse(searchVenue.PlaceVisitDate).Date.ToString()).Count() == 0)

                  {

                    #region Check budget limit with refernce to adults number
                    if (model.BudgetRange == BudgetRange.First)
                    {
                      if ((daybydaylist.Sum(x => x.PriceInDollar)) * model.NoOfAdults < 1000)
                      {
                        daybydaylist.Add(searchVenue);
                      }
                      else
                        break;
                    }
                    if (model.BudgetRange == BudgetRange.Second)
                    {
                      if ((daybydaylist.Sum(x => x.PriceInDollar)) * model.NoOfAdults < 3000)
                      {
                        daybydaylist.Add(searchVenue);
                      }
                      else
                        break;
                    }
                    if (model.BudgetRange == BudgetRange.Third)
                    {
                      if ((daybydaylist.Sum(x => x.PriceInDollar)) * model.NoOfAdults < 5000)
                      {
                        daybydaylist.Add(searchVenue);
                      }
                      else
                        break;
                    }
                    if (model.BudgetRange == BudgetRange.Fourth)
                    {
                      daybydaylist.Add(searchVenue);
                    }
                    #endregion
                  }
                  #endregion

                  else
                  {
                    daybydaylist.Add(searchVenue);
                  }
                }
                #endregion

              }

              else
              {
                model.StartDate = model.StartDate.AddDays(1);
                startTime = TimeSpan.Parse(startTimeOfDay + ":00");
                var totalPlaceVisitDuration = allPlaces[entryIndex].PlaceVisitDuration.Split(":");
                int totalPlaceVisitDurationMinutes;
                if (totalPlaceVisitDuration.Count() == 1)
                {
                  try { totalPlaceVisitDurationMinutes = (int.Parse(totalPlaceVisitDuration[0]) * 60); }
                  catch (Exception ex)
                  {
                    totalPlaceVisitDurationMinutes = getDurationInMins(allPlaces[entryIndex].Speed, fastPacedDefaultTime, slowPacedDefaultTime, popularPacedDefaultTime);

                  }
                }
                else if (totalPlaceVisitDuration[0] == "")
                {
                  try { totalPlaceVisitDurationMinutes = (int.Parse(totalPlaceVisitDuration[1])); }
                  catch (Exception ex)
                  {
                    totalPlaceVisitDurationMinutes = getDurationInMins(allPlaces[entryIndex].Speed, fastPacedDefaultTime, slowPacedDefaultTime, popularPacedDefaultTime);

                  }
                }
                else
                  try { totalPlaceVisitDurationMinutes = (int.Parse(totalPlaceVisitDuration[0]) * 60) + (int.Parse(totalPlaceVisitDuration[1])); }
                  catch (Exception ex)
                  {
                    totalPlaceVisitDurationMinutes = getDurationInMins(allPlaces[entryIndex].Speed, fastPacedDefaultTime, slowPacedDefaultTime, popularPacedDefaultTime);

                  }
                var time1 = totalPlaceVisitDurationMinutes / 60;
                var time2 = totalPlaceVisitDurationMinutes % 60;

                var secondTime = string.Format("{0:00}:{1:00}", time1, time2);
                TimeSpan t2 = TimeSpan.Parse(secondTime);

                TimeSpan endTime = startTime.Add(t2);

                placesToAdd.Add(daybydaylist);
                daybydaylist = new List<SearchVenue>();
                SearchVenue searchVenue = new SearchVenue();

                searchVenue.CurrencyExchangeRate = allPlaces[entryIndex].CurrencyExchangeRate;
                searchVenue.CityId = allPlaces[entryIndex].CityId;
                searchVenue.EventId = allPlaces[entryIndex].EventId;
                searchVenue.CategoryName = allPlaces[entryIndex].CategoryName;
                searchVenue.EventName = allPlaces[entryIndex].EventName;
                searchVenue.EventDescription = allPlaces[entryIndex].EventDescription;
                searchVenue.EventSlug = allPlaces[entryIndex].EventSlug;
                searchVenue.Latitude = allPlaces[entryIndex].Latitude;
                searchVenue.Longitude = allPlaces[entryIndex].Longitude;
                searchVenue.CityName = allPlaces[entryIndex].CityName;
                searchVenue.Currency = allPlaces[entryIndex].Currency;
                searchVenue.EventAltId = allPlaces[entryIndex].EventAltId;
                searchVenue.PlaceVisitDuration = allPlaces[entryIndex].PlaceVisitDuration;
                searchVenue.StartTime = startTime.ToString();
                searchVenue.EndTime = endTime.ToString();
                searchVenue.PlaceVisitDate = model.StartDate.Date.ToString("yyyy-MM-dd");

                searchVenue.TravelDuration = "0";
                searchVenue.Image = allPlaces[entryIndex].Image;
                searchVenue.Price = allPlaces[entryIndex].Price;

                searchVenue.PriceInDollar = allPlaces[entryIndex].PriceInDollar;
                #region Check Place off day
                if (allPlaces[entryIndex].PlaceOffDaysList != null && allPlaces[entryIndex].PlaceOffDaysList.Count > 0)
                {
                  if (allPlaces[entryIndex].PlaceOffDaysList.Where
                      (x => x.WeekOffDay.ToString() == DateTime.Parse(searchVenue.PlaceVisitDate).DayOfWeek.ToString()).Count() > 0
                      ||
                      allPlaces[entryIndex].PlaceOffDaysList.Where
                      (x => x.WeekOffDay.ToString() == "All").Count() > 0
                      )
                  {
                    if (allPlaces[entryIndex].PlaceHolidaysList.Where
                   (x => x.LeaveDateTime.ToString() == DateTime.Parse(searchVenue.PlaceVisitDate).Date.ToString()).Count() == 0)

                    {


                      #region Check budget limit with refernce to adults number
                      if (model.BudgetRange == BudgetRange.First)
                      {
                        if ((daybydaylist.Sum(x => x.PriceInDollar)) * model.NoOfAdults < 1000)
                        {
                          daybydaylist.Add(searchVenue);
                        }
                        else
                          break;
                      }
                      if (model.BudgetRange == BudgetRange.Second)
                      {
                        if ((daybydaylist.Sum(x => x.PriceInDollar)) * model.NoOfAdults < 3000)
                        {
                          daybydaylist.Add(searchVenue);
                        }
                        else
                          break;
                      }
                      if (model.BudgetRange == BudgetRange.Third)
                      {
                        if ((daybydaylist.Sum(x => x.PriceInDollar)) * model.NoOfAdults < 5000)
                        {
                          daybydaylist.Add(searchVenue);
                        }
                        else
                          break;
                      }
                      if (model.BudgetRange == BudgetRange.Fourth)
                      {
                        daybydaylist.Add(searchVenue);
                      }
                      #endregion

                    }
                  }
                }
                #endregion
                //startTime = endTime;
                if (totalDurationMinutes > totalMinutesInFourHrs)
                {
                  daybydaylist.Add(searchVenue);
                }
                totalCalculatedMins = 0;
              }
              var totalMinsArr = allPlaces[entryIndex].PlaceVisitDuration.Split(":");
              int totalMinutes;
              if (totalMinsArr.Count() == 1)
              {
                try { totalMinutes = (int.Parse(totalMinsArr[0]) * 60); }
                catch (Exception ex)
                {
                  totalMinutes = getDurationInMins(allPlaces[entryIndex].Speed, fastPacedDefaultTime, slowPacedDefaultTime, popularPacedDefaultTime);

                }
              }
              else if (totalMinsArr[0] == "")
              {
                try { totalMinutes = (int.Parse(totalMinsArr[1])); }
                catch (Exception ex)
                {
                  totalMinutes = getDurationInMins(allPlaces[entryIndex].Speed, fastPacedDefaultTime, slowPacedDefaultTime, popularPacedDefaultTime);

                }
              }
              else
                try { totalMinutes = (int.Parse(totalMinsArr[0]) * 60) + (int.Parse(totalMinsArr[1])); }
                catch (Exception ex)
                {
                  totalMinutes = getDurationInMins(allPlaces[entryIndex].Speed, fastPacedDefaultTime, slowPacedDefaultTime, popularPacedDefaultTime);

                }

              totalCalculatedMins = totalCalculatedMins + durationTime + totalMinutes;
              startingPoint = foundPlace;
              count = count + durationTime + totalMinutes;
              allPlaces.RemoveAt(entryIndex);

            }

            else
            {
              if (daybydaylist.Count > 0)
              {
                placesToAdd.Add(daybydaylist);
              }
              break;
            }
          }
          else
          {
            if (placeCount < allPlaces.Count() - 1)
              startingPoint = allPlaces[placeCount + 1];
            else
              break;
          }
        }


        //if (daybydaylist.Count() > 0)
        //    placesToAdd.Add(daybydaylist);

      }

      return placesToAdd;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("api/event/uploadimage")]
    public async Task<List<ImageModel>> UploadImage()
    {
      try
      {
        var session = await _sessionProvider.Get();
        //var files = Request.Form["file"];
        //IFormFile file1 = (IFormFile)files;
        var files = Request.Form.Files;
        long size = files.Sum(f => f.Length);
        bool updateSuccess = false;
        List<ImageModel> altids = new List<ImageModel>();

        var previous = "";
        var next = "";
        int imageCount = 1;
        var guid = "";
        if (Request.Cookies["placeAltId"] != null)
        {
          guid = Request.Cookies["placeAltId"].ToString().ToUpper(); ;
        }
        else
        {
          guid = Guid.NewGuid().ToString().ToUpper();
          CookieOptions option = new CookieOptions();
          option.Expires = DateTime.Now.AddMinutes(60);
          Response.Cookies.Append("placeAltId", guid.ToString(), option);
        }
        string imageAltId = "";

        altids.Add(new ImageModel()
        {
          AltId = guid.ToString(),
          Name = imageAltId
        });
        foreach (var formFile in files)
        {
          next = formFile.FileName;
          if (next.Equals(previous))
          {
            imageCount++;
          }
          else
            imageCount = 1;

          previous = formFile.FileName;
          if (formFile.Length > 0)
          {

            try
            {
              Stream stream = formFile.OpenReadStream();
              var img = Bitmap.FromStream(stream);

              ImageType imageType = ImageType.FeelHotTicketSlider;

              if (formFile.FileName.ToUpper() == "TILES")
              {
                imageAltId = guid.ToString() + "-ht-c" + (imageCount).ToString() + "";
                imageType = ImageType.FeelHotTicketSlider;
              }
              else if (formFile.FileName.ToUpper() == "DESCBANNER")
              {
                imageAltId = guid.ToString() + "-about";
                imageType = ImageType.FeelDescriptionPage;
              }
              else if (formFile.FileName.ToUpper() == "INVENTORYBANNER")
              {
                imageAltId = guid.ToString() + "";
                imageType = ImageType.FeelInventoryPage;
              }
              else if (formFile.FileName.ToUpper() == "GALLERY")
              {
                imageAltId = guid.ToString() + "-glr-" + (imageCount).ToString() + "";
                imageType = ImageType.FeelGallery;
              }
              else if (formFile.FileName.ToUpper() == "PLACEMAPIMAGES")
              {
                imageAltId = guid.ToString() + "-place-plan-" + (imageCount).ToString() + "";
                imageType = ImageType.FeelPlacePlan;
              }
              else if (formFile.FileName.ToUpper() == "TIMELINEIMAGES")
              {
                imageAltId = guid.ToString() + "-timeline-" + (imageCount).ToString() + "";
                imageType = ImageType.FeelTimeline;
              }
              else if (formFile.FileName.ToUpper() == "IMMERSIVEIMAGES")
              {
                imageAltId = guid.ToString() + "-immersive-experience-" + (imageCount).ToString() + "";
                imageType = ImageType.FeelImmersiveExperience;
              }
              else if (formFile.FileName.ToUpper() == "ARCHDETAILIMAGES")
              {
                imageAltId = guid.ToString() + "-architectural-detail-" + (imageCount).ToString() + "";
                imageType = ImageType.FeelArchitecturalDetail;
              }

              //imageType = ImageType.FeelTest;
              _amazonS3FileProvider.UploadFeelImage(img, imageAltId, imageType);
              altids.Add(new ImageModel()
              {
                AltId = guid.ToString(),
                Name = imageAltId
              });
              updateSuccess = true;
            }
            catch
            {
              updateSuccess = false;
            }
            //System.IO.File.Delete(filePath);
          }
        }
        return altids;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    private async Task<LocationCommandResult> SavedLocation(SaveLocationViewModel model, long id)
    {
      LocationCommand command = new LocationCommand();
      command.PlaceName = model.Title;
      command.Title = model.Placename;
      command.Location = model.Location;
      command.Country = model.Country;
      command.State = model.State;
      command.City = model.City;
      command.Address1 = model.Address1;
      command.Address2 = model.Address2;
      //command.Latitude = model.Latitude;
      //command.logitude = model.Logitude;
      command.EventId = id;
      command.TilesSliderImages = model.TilesSliderImages;
      command.DescpagebannerImages = model.DescpagebannerImages;
      command.InventorypagebannerImage = model.InventorypagebannerImage;
      command.GalleryImages = model.GalleryImages;
      command.PlacemapImages = model.PlacemapImages;
      command.TimelineImages = model.TimelineImages;
      command.ArchdetailImages = model.ArchdetailImages;
      command.IsEdit = model.IsEdit;
      command.Lat = model.Lat;
      command.Long = model.Long;
      command.Zip = model.Zip;
      return await _commandSender.Send<LocationCommand, LocationCommandResult>(command);
    }

    private async Task<bool> SaveDescription(SaveEventDataViewModel model, long eventid)
    {
      var features = Enum.GetValues(typeof(LearnMoreFeatures));
      List<EventLearnMoreAttributeCommand> commands = new List<EventLearnMoreAttributeCommand>();
      var result = new { Success = true };
      foreach (var feature in features)
      {
        string desc = "";
        int tableId = 0;
        if (feature.Equals(LearnMoreFeatures.PlaceMap) || feature.Equals(LearnMoreFeatures.Timeline))
        {
          continue;
        }
        else if (feature.Equals(LearnMoreFeatures.History))
        {
          desc = model.History;
          tableId = model.HistoryId;
        }
        else if (feature.Equals(LearnMoreFeatures.ArchitecturalDetail))
        {
          desc = model.Archdetail;
          tableId = model.ArchdetailId;
        }
        else if (feature.Equals(LearnMoreFeatures.HighlightNugget))
        {
          desc = model.Highlights;
          tableId = model.HighlightsId;
        }
        else if (feature.Equals(LearnMoreFeatures.ImmersiveExperience))
        {
          desc = model.Immersiveexperience;
          tableId = model.ImmersiveexperienceId;
        }
        else
        {
          desc = model.Description;
        }
        EventLearnMoreAttributeCommand command = new EventLearnMoreAttributeCommand()
        {
          Id = tableId,
          AltId = Guid.NewGuid(),
          Description = desc,
          EventId = eventid,
          LearnMoreFeatureId = (LearnMoreFeatures)feature,
          Image = "NA",
          IsEnabled = true,
          ModifiedBy = Guid.NewGuid()
        };
        await _commandSender.Send<EventLearnMoreAttributeCommand>(command);
      }
      return result.Success;
    }


    public double getDistance(SearchVenue p1, GeoCoordinate p2)
    {
      var R = 6378137; // Earth’s mean radius in meter
      var dLat = rad(Convert.ToDouble(p2.Latitude) - Convert.ToDouble(p1.Latitude));
      var dLong = rad(Convert.ToDouble(p2.Longitude) - Convert.ToDouble(p1.Longitude));
      var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
        Math.Cos(rad(Convert.ToDouble(p1.Latitude)) * Math.Cos(rad(Convert.ToDouble(p2.Latitude)))) *
        Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
      var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
      var d = R * c;
      if (d > 0)
      {
        var abc = d;
      }
      return d; // returns the distance in meter
    }

    private double rad(double val)
    {
      return val * Math.PI / 180;
    }


    [HttpPost]
    [AllowAnonymous]
    [Route("api/eventdetail/save")]
    public async Task<EventDetailResponseViewModel> SaveEventDetail([FromBody] SaveEventDetailDataViewModel model)
    {
      try
      {
        //var eventResult = await _querySender.Send(new EventCreationQuery
        //{
        //    EventAltId = Guid.Parse(model.EventAltId)
        //});
        var veneueResult = await _querySender.Send(new VenueDataQuery
        {
          VenueAltId = Guid.Parse(model.VenueAltId)
        });

        SaveEventDetailDataResult EventDetailData = await _commandSender.Send<SaveEventDetailCommand, SaveEventDetailDataResult>(new SaveEventDetailCommand
        {
          Id = model.Id,
          Name = model.Name,
          StartDateTime = Convert.ToDateTime(model.StartDateTime),
          EndDateTime = Convert.ToDateTime(model.EndDateTime),
          EventId = model.EventId,
          VenueId = veneueResult.Venues.Id,
          MetaDetails = model.MetaDetails,
          IsEnabled = true,
          GroupId = 1 // Not passed through FrontEnd...
        });

        return new EventDetailResponseViewModel
        {
          Id = EventDetailData.Id,
          Success = true
        };
      }
      catch (Exception ex)
      {
        new EventDetailResponseViewModel
        {
          Success = false
        };
      }
      return new EventDetailResponseViewModel
      {
        Success = false
      };
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("api/financial/save")]
    public async Task<FinancialResponseViewModel> SaveFinancials([FromBody] FinanceRequestViewModel model)
    {

      try
      {

        //var eventResult = await _querySender.Send(new EventCreationQuery
        //{
        //    EventAltId = Guid.Parse(model.EventAltId)
        //});
        var veneueResult = await _querySender.Send(new VenueDataQuery
        {
          //VenueAltId = Guid.Parse(model.VenueAltId)
        });

        FinancDetailCommandResult FinancDetailData = await _commandSender.Send<FinancDetailCommand, FinancDetailCommandResult>(new FinancDetailCommand
        {

          Id = model.Id,
          CurrencyId = model.CurrencyId,
          CountryId = model.CountryId,
          AccountType = model.AccountType,
          FirstName = model.FirstName,
          LastName = model.LastName,
          BankAccountType = model.BankAcountType,
          BankStateId = model.StateId,
          RoutingNo = model.RoutingNo,
          BankName = model.BankName,
          GSTNo = model.GSTNo,
          AccountNo = model.AccountNo,
          PANNo = model.PANNo,
          AccountNickName = model.AccountNickName,
          IsBankAccountGST = model.IsBankAccountGST,
          IsUpdatLater = model.IsUpdatLater,
          FinancialsAccountBankAccountGSTInfo = model.FinancialsAccountBankAccountGSTInfo,
          EventId = model.EventId


        });



        SaveLocationViewModel sl = new SaveLocationViewModel();
        sl.Placename = model.Placename;
        sl.Location = model.Location;
        sl.State = model.State;
        sl.Country = model.Country;
        sl.City = model.City;
        sl.Address1 = model.Address1;
        sl.Address2 = model.Address2;
        sl.EventDetailId = model.EventDetailId;



        var isSavedLocation = await SavedLocation(sl, model.EventId);


        return new FinancialResponseViewModel
        {
          Id = FinancDetailData.Id,
          Success = true
        };
      }
      catch (Exception ex)
      {
        new EventDetailResponseViewModel
        {
          Success = false
        };
      }
      return new FinancialResponseViewModel
      {
        Success = false
      };
    }

    [HttpPost]
    [Route("api/subeventcategorydelete/all")]
    public async Task<DeleteSubeventResponseViewModel> DeleteAsync([FromBody] SubEventDeleteViewModel model)
    {
      if (ModelState.IsValid)
      {
        var result = new { Success = true };
        try
        {
          await _commandSender.Send(new DeleteSubevent
          {
            Id = model.Id,
          });
          return new DeleteSubeventResponseViewModel
          {
            Success = result.Success,
          };
        }
        catch (Exception ex)
        {
          return new DeleteSubeventResponseViewModel
          {
            Success = false
          };
        }
      }
      else
      {
        return new DeleteSubeventResponseViewModel
        {
          Success = false
        };
      }
    }

    [HttpPost]
    [Route("api/eventsata/all")]
    public async Task<GetEventsResponseViewModel> GetEventsdata([FromBody] GetEventsDataViewModel model)
    {

      var queryResult = await _querySender.Send(new GetEventQuery
      {
        UserId = model.UserId
      });

      return new GetEventsResponseViewModel
      {
        Event = queryResult.Event
      };
    }

    [HttpPost]
    [Route("api/subeventcategory/all")]
    public async Task<SubEventDetailDataResponseViewModel> GetAllSubEvents([FromBody] GetsubeventViewModel model)
    {

      var queryResult = await _querySender.Send(new SubEventDetailQuery
      {
        EventId = model.EventId
      });

      return new SubEventDetailDataResponseViewModel
      {
        EventDetail = queryResult.EventDetail,
      };
    }
  }

  public class ImageModel
  {
    public string AltId { get; set; }
    public string Name { get; set; }
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
}
