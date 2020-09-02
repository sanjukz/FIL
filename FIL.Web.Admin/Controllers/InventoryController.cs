using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.CurrencyTypes;
using FIL.Contracts.Queries.RefundPolicy;
using FIL.Contracts.Queries.CustomerDocumentType;
using FIL.Contracts.Queries.CustomerInformations;
using FIL.Contracts.Queries.PlaceInventory;
using FIL.Contracts.Queries.TicketCategoryTypes;
using FIL.Foundation.Senders;
using FIL.Web.Kitms.Feel.ViewModels.Inventory;
using FIL.Web.Kitms.Feel.ViewModels.TicketCategoryTypes;
using FIL.Contracts.Commands.PlaceInventory;
using FIL.Contracts.Commands.CustomerIdType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FIL.Contracts.Enums;
using FIL.Contracts.Commands.PlaceCalendar;
using FIL.Web.Kitms.Feel.ViewModels.PlaceCalendar;
using FIL.Contracts.Commands.EventCreation;
using FIL.Web.Core.Providers;
using FIL.Contracts.Commands.Location;
using FIL.Contracts.Commands.Payment;
using FIL.Messaging.Senders;
using FIL.Web.Core.UrlsProvider;
using FIL.Messaging.Models.Emails;
using System.Text;
using Gma.QrCodeNet.Encoding.DataEncodation;
using FIL.Web.Kitms.Feel.ViewModels.Finance;
using FIL.Contracts.Commands.FinanceDetails;
using FIL.Contracts.Commands.CreateEventV1;
using FIL.Web.Kitms.Feel.ViewModels.CreateEventV1;

namespace FIL.Web.Kitms.Feel.Controllers
{
  public class InventoryController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;
    private readonly IAccountEmailSender _accountEmailSender;

    public InventoryController(
        ICommandSender commandSender,
        ISessionProvider sessionProvider,
        IAccountEmailSender accountEmailSender,
        IQuerySender querySender)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _sessionProvider = sessionProvider;
      _accountEmailSender = accountEmailSender;
    }

    [HttpGet]
    [Route("api/get/Currencies")]
    public async Task<CurrencyTypesResponseViewModel> GetCurrency()
    {
      try
      {
        var queryResult = await _querySender.Send(new CurrencyTypesQuery { });
        return new CurrencyTypesResponseViewModel
        {
          CurrencyTypes = queryResult.currencyTypes
        };
      }
      catch (Exception e)
      {
        return new CurrencyTypesResponseViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/ticketCategoryTypes")]
    public async Task<TicketCategoryTypesResponseViewModel> GetTicketCategoryType()
    {
      try
      {
        var queryResult = await _querySender.Send(new TicketCategoryTypesQuery { });
        return new TicketCategoryTypesResponseViewModel
        {
          TicketCategoryTypes = queryResult.TicketCategoryTypes,
          TicketCategorySubTypes = queryResult.TicketCategorySubTypes
        };
      }
      catch (Exception e)
      {
        return new TicketCategoryTypesResponseViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/refundPolicy")]
    public async Task<RefundPolicyResponseViewModel> GetRefundPolicy()
    {
      try
      {
        var queryResult = await _querySender.Send(new RefundPolicyQuery { });
        return new RefundPolicyResponseViewModel
        {
          RefundPolicies = queryResult.RefundPolicies
        };
      }
      catch (Exception e)
      {
        return new RefundPolicyResponseViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/DocumentTypes")]
    public async Task<DocumentTypeResponseViewModel> GetDocumentTypes()
    {
      try
      {
        var queryResult = await _querySender.Send(new CustomerDocumentTypeQuery { });
        return new DocumentTypeResponseViewModel
        {
          DocumentTypes = queryResult.CustomerDocumentTypes
        };
      }
      catch (Exception e)
      {
        return new DocumentTypeResponseViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/customerInformationControl")]
    public async Task<CustomerInformationControlResponseViewModel> GetCustomerInformationControl()
    {
      try
      {
        var queryResult = await _querySender.Send(new CustomerInformationsQuery { });
        return new CustomerInformationControlResponseViewModel
        {
          CustomerInformationControls = queryResult.CustomerInformations
        };
      }
      catch (Exception e)
      {
        return new CustomerInformationControlResponseViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/DeliveryTypes")]
    public async Task<DeliveryTypeResponseViewModel> GetDeliveryTypes()
    {
      try
      {
        List<string> deliveryTypeList = new List<string>();
        foreach (FIL.Contracts.Enums.DeliveryTypes deliveryTypes in Enum.GetValues(typeof(DeliveryTypes)))
        {
          deliveryTypeList.Add(deliveryTypes.ToString());
        }
        return new DeliveryTypeResponseViewModel
        {
          DeliveryTypes = deliveryTypeList
        };
      }
      catch (Exception e)
      {
        return new DeliveryTypeResponseViewModel { };
      }
    }

    [HttpPost]
    [Route("api/save/inventory")]
    public async Task<InventoryResponseViewModel> SavePlaceCalendar([FromBody] InventoryRequestViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          PlaceInventoryCommandCommandResult placeInventoryResult = await _commandSender.Send<PlaceInventoryCommand, PlaceInventoryCommandCommandResult>(new PlaceInventoryCommand
          {
            ticketCategoriesViewModels = AutoMapper.Mapper.Map<List<FIL.Contracts.Commands.PlaceInventory.TicketCategoriesViewModel>>(model.ticketCategoriesViewModels),
            EventDetailAltId = model.eventDetailAltIds,
            RedemptionAddress = model.RedemptionAddress,
            PlaceAltId = model.PlaceAltId,
            TermsAndCondition = model.TermsAndCondition,
            CustomerIdTypes = model.CustomerIdTypes,
            DeliverType = model.deliverTypeId,
            RedemptionDateTime = model.RedemptionDateTime,
            IsCustomerIdRequired = model.IsCustomerIdRequired,
            RedemptionCity = model.RedemptionCity,
            RedemptionCountry = model.RedemptionCountry,
            RedemptionState = model.RedemptionState,
            RedemptionZipcode = model.RedemptionZipcode,
            CustomerInformation = model.CustomerInformation,
            RedemptionInstructions = model.RedemptionInstructions,
            RefundPolicy = model.RefundPolicy,
            IsEdit = model.IsEdit,
            FeeTypes = AutoMapper.Mapper.Map<List<FIL.Contracts.Commands.PlaceInventory.FeeTypes>>(model.FeeTypes)
          });
          return new InventoryResponseViewModel
          {
            Success = true
          };
        }
        catch (Exception e)
        {
          return new InventoryResponseViewModel { };
        }
      }
      else
      {
        return new InventoryResponseViewModel { };
      }
    }

    [HttpPost]
    [Route("api/save/customerIdType")]
    public async Task<DocumentTypeSaveResponseViewModel> saveCustomerIdType([FromBody] DocumentTypeRequestViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var result = new { Succeeded = true };

          await _commandSender.Send(new CustomerIdTypeCommand
          {
            CustomerIdType = model.DocumentType
          });
          if (result.Succeeded)
          {
            return new DocumentTypeSaveResponseViewModel
            {
              Success = true
            };
          }
          else
          {
            return new DocumentTypeSaveResponseViewModel
            {
              Success = false
            };
          }
        }
        catch (Exception e)
        {
          return new DocumentTypeSaveResponseViewModel
          {
            Success = false
          };
        };
      }
      else
      {
        return new DocumentTypeSaveResponseViewModel { };
      }
    }

    [HttpGet]
    [Route("api/get/inventory/{placeAltId}/{isLiveOnline?}")]
    public async Task<GetPlaceInventoryDataResponseViewModel> GetByName(Guid placeAltId, bool? isLiveOnline = false)
    {
      var queryResult = await _querySender.Send(new GetPlaceInventoryQuery { PlaceAltId = placeAltId, IsLiveOnline = (bool)isLiveOnline });

      return new GetPlaceInventoryDataResponseViewModel
      {
        CustomerDocumentTypes = queryResult.CustomerDocumentTypes,
        DeliveryTypes = queryResult.DeliveryTypes,
        Event = queryResult.Event,
        eventDeliveryTypeDetails = queryResult.eventDeliveryTypeDetails,
        EventDetails = queryResult.EventDetails,
        PlaceCustomerDocumentTypeMappings = queryResult.PlaceCustomerDocumentTypeMappings,
        PlaceTicketRedemptionDetails = queryResult.PlaceTicketRedemptionDetails,
        TicketCategoryContainer = AutoMapper.Mapper.Map<List<TicketCategoryInfo>>(queryResult.TicketCategoryContainer),
        TicketValidityTypes = queryResult.TicketValidityTypes,
        PlaceWeekOffs = queryResult.PlaceWeekOffs,
        PlaceHolidayDates = queryResult.PlaceHolidayDates,
        EventCustomerInformationMappings = queryResult.EventCustomerInformationMappings,
        CustomerInformations = queryResult.CustomerInformations,
        EventTicketDetailTicketCategoryTypeMappings = queryResult.EventTicketDetailTicketCategoryTypeMappings,
        RegularTimeModel = queryResult.RegularTimeModel,
        SeasonTimeModel = queryResult.SeasonTimeModel,
        SpecialDayModel = queryResult.SpecialDayModel,
        EventAttribute = queryResult.EventAttribute
      };
    }

    [HttpPost]
    [Route("api/save/eventDetail")]
    public async Task<InventoryResponseViewModel> saveEventDetailCreation([FromBody] EventDetailCreationViewModel model)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var session = await _sessionProvider.Get();
          var startDateTime = new DateTime(model.EventCalendar.PlaceStartDate.Year, model.EventCalendar.PlaceStartDate.Month, model.EventCalendar.PlaceStartDate.Day, Convert.ToInt32(model.EventCalendar.RegularTimeModel.TimeModel.FirstOrDefault().From.Split(":")[0]), Convert.ToInt32(model.EventCalendar.RegularTimeModel.TimeModel.FirstOrDefault().From.Split(":")[1]), 0);
          var EndDateTime = new DateTime(model.EventCalendar.PlaceStartDate.Year, model.EventCalendar.PlaceStartDate.Month, model.EventCalendar.PlaceStartDate.Day, Convert.ToInt32(model.EventCalendar.RegularTimeModel.TimeModel.FirstOrDefault().To.Split(":")[0]), Convert.ToInt32(model.EventCalendar.RegularTimeModel.TimeModel.FirstOrDefault().To.Split(":")[1]), 0);
          if (!model.EventCalendar.TimeZone.Contains("-"))
          {
            model.EventCalendar.TimeZone = "+" + model.EventCalendar.TimeZone;
          }
          if (model.EventCalendar.TimeZone != null && model.EventCalendar.TimeZone.Contains("+"))
          {
            var time = Convert.ToInt64(model.EventCalendar.TimeZone.Replace("+", ""));
            startDateTime = startDateTime.AddMinutes(-time);
            EndDateTime = EndDateTime.AddMinutes(-time);
          }
          else if (model.EventCalendar.TimeZone != null && model.EventCalendar.TimeZone.Contains("-"))
          {
            var time = Convert.ToInt64(model.EventCalendar.TimeZone.Replace("-", ""));
            startDateTime = startDateTime.AddMinutes(time);
            EndDateTime = EndDateTime.AddMinutes(time);
          }

          SaveEventDataResult EventData = await _commandSender.Send<SaveEventCommand, SaveEventDataResult>(new SaveEventCommand
          {
            IsEventCreation = true,
            Id = model.EventId,
            AltId = Guid.NewGuid(),
            Name = model.Title,
            Description = model.EventDescription,
            TermsAndConditions = "NA",
            EventCategoryId = 98,
            EventType = (EventType)1,
            MetaDetails = "",
            IsEnabled = true,
            IsFeel = true,
            ClientPointOfContactId = 1,
            TagIds = "",
            SubCategoryIds = model.EventCategoryId,
            AmenityIds = "",
            TimeDuration = "02:00-04:00",
            IsEdit = model.IsEdit,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC"),
          });
          if (!model.IsEdit)
          {
            await _commandSender.Send<LocationCommand, LocationCommandResult>(new LocationCommand
            {
              PlaceName = model.Title,
              Title = model.Title,
              Location = "1-5 Wheat Road, Darling Harbour",
              Country = "Australia",
              State = "New South Wales",
              City = "Sydney",
              Address1 = "1-5 Wheat Road, Darling Harbour",
              Address2 = "1-5 Wheat Road, Darling Harbour",
              EventId = EventData.Id,
              IsEdit = model.IsEdit,
              Lat = "-33.869598",
              Long = "151.201577",
              Zip = "411028",
              TilesSliderImages = "",
              DescpagebannerImages = "",
              InventorypagebannerImage = "",
              GalleryImages = "",
              PlacemapImages = "",
              TimelineImages = "",
              ArchdetailImages = "",
              ParentCategoryId = 98,
              TimeZoneAbbreviation = model.EventCalendar.TimeZoneAbbreviation,
              TimeZone = model.EventCalendar.TimeZone
            });

            PlaceCalendarCommandResult placeCalendarResult = await _commandSender.Send<PlaceCalendarCommand, PlaceCalendarCommandResult>(new PlaceCalendarCommand
            {
              IsEventCreation = true,
              PlaceAltId = EventData.AltId,
              VenueAltId = model.EventCalendar.VenueAltId,
              HolidayDates = model.EventCalendar.HolidayDates,
              WeekOffDays = model.EventCalendar.WeekOffDays,
              PlaceStartDate = startDateTime,
              PlaceEndDate = EndDateTime,
              PlaceType = model.EventCalendar.PlaceType,
              IsEdit = model.EventCalendar.IsEdit,
              IsNewCalendar = model.EventCalendar.IsNewCalendar,
              PlaceTimings = AutoMapper.Mapper.Map<List<FIL.Contracts.Commands.PlaceCalendar.Timing>>(model.EventCalendar.PlaceTimings),
              RegularTimeModel = AutoMapper.Mapper.Map<FIL.Contracts.Commands.PlaceCalendar.RegularViewModel>(model.EventCalendar.RegularTimeModel),
              SeasonTimeModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Commands.PlaceCalendar.SeasonViewModel>>(model.EventCalendar.SeasonTimeModel),
              SpecialDayModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Commands.PlaceCalendar.SpecialDayViewModel>>(model.EventCalendar.SpecialDayModel),
            });
          }
          var eventHostCommandResult = await _commandSender.Send<EventHostCommand, EventHostCommandResult>(new EventHostCommand
          {
            EventHostMappings = model.EventHosts,
            EventId = EventData.Id,
            ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
          });

          return new InventoryResponseViewModel
          {
            Success = true,
            EventAltId = EventData.AltId,
            EventHosts = eventHostCommandResult.EventHostMappings
          };
        }
        catch (Exception e)
        {
          return new InventoryResponseViewModel { };
        }
      }
      else
      {
        return new InventoryResponseViewModel { };
      }
    }






    public async Task<bool> SaveStripeConnect(Guid eventId, string auth_Code, bool isStripeConnect)
    {
      try
      {
        var session = await _sessionProvider.Get();
        StripeConnectAccountCommandResult EventData = await _commandSender.Send<StripeConnectAccountCommand, StripeConnectAccountCommandResult>(new StripeConnectAccountCommand
        {
          AuthorizationCode = auth_Code,
          ExtraCommisionPercentage = 25,
          ExtraCommisionFlat = 0,
          channels = Channels.Feel,
          EventId = eventId,
          IsStripeConnect = isStripeConnect,
          ModifiedBy = session.User != null ? session.User.AltId : Guid.Parse("7390283B-4A32-4860-BA3D-B57F1E5F2DAC")
        });
        if (EventData.Success)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      catch (Exception e)
      {
        return false;
      }
    }

    [HttpGet]
    [Route("api/save/stripeConnectAcccountIds/{auth_Code}/{eventId}")]
    public async Task<InventoryResponseViewModel> SaveStripeConnectAccountId(string auth_Code, Guid eventId)
    {
      try
      {
        var isSuccess = await SaveStripeConnect(eventId, auth_Code, true);
        if (isSuccess)
        {
          return new InventoryResponseViewModel
          {
            Success = true
          };
        }
        else
        {
          return new InventoryResponseViewModel { };
        }
      }
      catch (Exception e)
      {
        return new InventoryResponseViewModel { };
      }
    }
  }
}

