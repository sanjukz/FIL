using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FIL.Contracts.Commands.EventCategoryMapping;
using FIL.Contracts.Commands.EventSiteIdMapping;
using FIL.Contracts.Queries.Events;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Admin.ViewModels.Event;
using FIL.Web.Admin.ViewModels.SiteInfo;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FIL.Web.Admin.Controllers
{
  public class EventController : Controller
  {
    private readonly ICommandSender _commandSender;
    private readonly IQuerySender _querySender;
    private readonly ISessionProvider _sessionProvider;

    public EventController(ICommandSender commandSender, IQuerySender querySender, ISessionProvider sessionProvider)
    {
      _commandSender = commandSender;
      _querySender = querySender;
      _sessionProvider = sessionProvider;
    }
    [HttpGet]
    [Route("api/event")]
    public async Task<EventTicketDataViewModel> GetEventData()
    {
      var session = await _sessionProvider.Get();
      if (session.IsAuthenticated)
      {

        var queryResult = await _querySender.Send(new EventsFeelQuery { IsFeel = true, UserAltId = session.User.AltId, RoleId = (int)session.User.RolesId });
        queryResult.Events = queryResult.Events.ToList();
        return new EventTicketDataViewModel()
        {
          Venues = queryResult.Events.Select(p => new EventDataViewModel()
          {
            Id = p.Id,
            AltId = p.AltId,
            Description = p.Description,
            EventCategoryId = p.EventCategoryId,
            EventSourceId = p.EventSourceId,
            EventTypeId = p.EventTypeId,
            ImagePath = p.ImagePath,
            IsEnabled = p.IsEnabled,
            IsFeel = p.IsFeel,
            IsPublishedOnSite = p.IsPublishedOnSite,
            PublishedDateTime = p.PublishedDateTime,
            Name = p.Name,
          }).ToList()
        };
      }
      else
      {
        return new EventTicketDataViewModel()
        {
        };
      }

    }

    [HttpGet]
    [Route("api/eventsearch/{searchdata}")]
    public async Task<EventTicketDataViewModel> SearchEventData(string searchdata)
    {
      var session = await _sessionProvider.Get();
      if (session.IsAuthenticated)
      {
        var queryResult = await _querySender.Send(new EventsFeelSearchQuery { IsFeel = true, SearchString = searchdata, UserAltId = session.User.AltId, RoleId = (int)session.User.RolesId });
        queryResult.Events = queryResult.Events.ToList();
        return new EventTicketDataViewModel()
        {
          Venues = queryResult.Events.Select(p => new EventDataViewModel()
          {
            Id = p.Id,
            AltId = p.AltId,
            Description = p.Description,
            EventCategoryId = p.EventCategoryId,
            EventSourceId = p.EventSourceId,
            EventTypeId = p.EventTypeId,
            ImagePath = p.ImagePath,
            IsEnabled = p.IsEnabled,
            IsFeel = p.IsFeel,
            IsPublishedOnSite = p.IsPublishedOnSite,
            PublishedDateTime = p.PublishedDateTime,
            Name = p.Name,
          }).ToList()
        };
      }
      else
      {
        return new EventTicketDataViewModel()
        {
        };
      }

    }

    [HttpGet]
    [Route("api/event/categories")]
    public async Task<EventCategoryDataViewModel> GetEventCategories()
    {
      EventCategoryDataViewModel categoryVM = new EventCategoryDataViewModel();
      categoryVM.categories = new List<CategoryDataViewModel>();
      var queryResult = await _querySender.Send(new EventsCategoryFeelQuery { IsEnabled = true });
      foreach (var p in queryResult.EventCategoryFeel)
      {
        CategoryDataViewModel data = new CategoryDataViewModel()
        {
          CategoryId = p.CategoryId,
          DisplayName = p.DisplayName,
          IsFeel = p.IsFeel,
          IsHomePage = p.IsHomePage,
          Order = p.Order,
          Slug = p.Slug,
          Value = p.Value,
          MasterEventTypeId = p.MasterEventTypeId
        };
        categoryVM.categories.Add(data);
      }
      return categoryVM;
    }

    [HttpGet]
    [Route("api/event/categorymapping/{eventId}")]
    public async Task<EventCategoryMappingDataViewModel> GetEventCategoryMapping(int eventId)
    {
      EventCategoryMappingDataViewModel categoryVM = new EventCategoryMappingDataViewModel();
      categoryVM.eventcatmapping = new List<EventCategoryMappingViewModel>();
      var queryResult = await _querySender.Send(new EventCategoryMappingQuery { EventId = eventId });
      foreach (var p in queryResult.EventCategoryMappings)
      {
        EventCategoryMappingViewModel data = new EventCategoryMappingViewModel()
        {
          CreatedBy = p.CreatedBy,
          CreatedUtc = p.CreatedUtc,
          EventCategoryId = p.EventCategoryId,
          EventId = p.EventId,
          Id = p.Id,
          IsEnabled = p.IsEnabled,
          UpdatedBy = p.UpdatedBy,
          UpdatedUtc = p.UpdatedUtc
        };
        categoryVM.eventcatmapping.Add(data);
      }
      return categoryVM;
    }

    [HttpPost]
    [Route("api/event/map")]
    public async Task<IActionResult> UpdateEventCategories([FromBody] UpdateEventCategoryMapViewModel updateVM)
    {
      var result = new { Succeeded = true };

      await _commandSender.Send(new EventCategoryMappingCommand()
      {
        Categoryid = updateVM.Subcategoryid,
        Eventid = updateVM.Eventid,
        Isenabled = updateVM.Isenabled,
        Id = updateVM.Id
      });
      if (result.Succeeded)
      {
        return Ok(true);
      }
      else
      {

        return BadRequest(false);
      }
    }

    [HttpGet]
    [Route("api/event/siteidmapping/{eventId}")]
    public async Task<EventSiteIdMappingDataViewModel> GetSiteIdMapping(int eventId)
    {
      EventSiteIdMappingDataViewModel siteIdVM = new EventSiteIdMappingDataViewModel();
      siteIdVM.eventsiteidmapping = new List<EventSiteIdMappingViewModel>();
      var queryResult = await _querySender.Send(new EventSiteIdMappingQuery { EventId = eventId });
      foreach (var p in queryResult.EventSiteIdMappings)
      {
        EventSiteIdMappingViewModel data = new EventSiteIdMappingViewModel()
        {
          CreatedBy = p.CreatedBy,
          CreatedUtc = p.CreatedUtc,
          EventId = p.EventId,
          Id = p.Id,
          IsEnabled = p.IsEnabled,
          UpdatedBy = p.UpdatedBy,
          UpdatedUtc = p.UpdatedUtc,
          SiteId = p.SiteId,
          SortOrder = p.SortOrder
        };
        siteIdVM.eventsiteidmapping.Add(data);
      }
      return siteIdVM;
    }

    [HttpPost]
    [Route("api/event/sitemap")]
    public async Task<IActionResult> UpdateSiteId([FromBody] UpdateSiteMapIdViewModel updateVM)
    {
      var result = new { Succeeded = true };
      var queryResult = await _querySender.Send(new EventSiteIdMappingSortQuery { SiteId = updateVM.SiteId });
      bool IsAdded = false;
      int NewPosition = 0;
      int PreviousPosition = 0;

      for (int i = 0; i < queryResult.EventSiteIdMappings.Count; i++)
      {
        var data = queryResult.EventSiteIdMappings[i];
        if (updateVM.Id == 0)
        {
          if (queryResult.EventSiteIdMappings[i].SortOrder == updateVM.SortOrder)
          {
            await _commandSender.Send(new EventSiteIdMappingCommand()
            {
              EventId = updateVM.EventId,
              IsEnabled = updateVM.IsEnabled,
              SiteId = updateVM.SiteId,
              SortOrder = updateVM.SortOrder,
              Id = updateVM.Id
            });
            IsAdded = true;
          }

          if (IsAdded)
          {
            short increment = (Int16)(1 + data.SortOrder);
            await _commandSender.Send(new EventSiteIdMappingCommand()
            {
              EventId = data.EventId,
              IsEnabled = data.IsEnabled,
              SiteId = data.SiteId,
              SortOrder = increment,
              Id = data.Id
            });
          }
        }
        else if (data.Id == updateVM.Id)
        {
          NewPosition = updateVM.SortOrder;
          PreviousPosition = data.SortOrder;
          await _commandSender.Send(new EventSiteIdMappingCommand()
          {
            EventId = updateVM.EventId,
            IsEnabled = updateVM.IsEnabled,
            SiteId = updateVM.SiteId,
            SortOrder = updateVM.SortOrder,
            Id = updateVM.Id
          });
          break;
        }
      }
      if (updateVM.Id > 0)
      {
        for (int i = 0; i < queryResult.EventSiteIdMappings.Count; i++)
        {
          var data = queryResult.EventSiteIdMappings[i];

          if (NewPosition < PreviousPosition && data.SortOrder >= NewPosition && data.Id != updateVM.Id && data.SortOrder <= PreviousPosition)
          {
            short increment = (Int16)(1 + data.SortOrder);
            await _commandSender.Send(new EventSiteIdMappingCommand()
            {
              EventId = data.EventId,
              IsEnabled = data.IsEnabled,
              SiteId = data.SiteId,
              SortOrder = increment,
              Id = data.Id
            });

          }
          else if (NewPosition > PreviousPosition && data.SortOrder >= PreviousPosition && data.Id != updateVM.Id && data.SortOrder <= NewPosition)
          {
            short increment = (Int16)(data.SortOrder - 1);
            await _commandSender.Send(new EventSiteIdMappingCommand()
            {
              EventId = data.EventId,
              IsEnabled = data.IsEnabled,
              SiteId = data.SiteId,
              SortOrder = increment,
              Id = data.Id
            });

          }
        }
      }

      if (result.Succeeded)
      {
        return Ok(true);
      }
      else
      {

        return BadRequest(false);
      }
    }

  }
}
