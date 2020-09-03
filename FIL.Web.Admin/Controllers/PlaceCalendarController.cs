using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Zipcode;
using FIL.Foundation.Senders;
using FIL.Web.Admin.ViewModels.PlaceCalendar;
using FIL.Contracts.Commands.PlaceCalendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Admin.Controllers
{
    public class PlaceCalendarController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;

        public PlaceCalendarController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpPost]
        [Route("api/save/calendar")]
        public async Task<PlaceCalendarResponseViewModel> SavePlaceCalendar([FromBody]PlaceCalendarRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PlaceCalendarCommandResult placeCalendarResult = await _commandSender.Send<PlaceCalendarCommand, PlaceCalendarCommandResult>(new PlaceCalendarCommand
                    {
                        PlaceAltId = model.PlaceAltId,
                        VenueAltId = model.VenueAltId,
                        HolidayDates = model.HolidayDates,
                        WeekOffDays = model.WeekOffDays,
                        PlaceStartDate = model.PlaceStartDate,
                        PlaceEndDate = model.PlaceEndDate,
                        PlaceType = model.PlaceType,
                        IsEdit = model.IsEdit,
                        IsNewCalendar = model.IsNewCalendar,
                        PlaceTimings = AutoMapper.Mapper.Map<List<FIL.Contracts.Commands.PlaceCalendar.Timing>>(model.PlaceTimings),
                        RegularTimeModel = AutoMapper.Mapper.Map<FIL.Contracts.Commands.PlaceCalendar.RegularViewModel>(model.RegularTimeModel),
                        SeasonTimeModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Commands.PlaceCalendar.SeasonViewModel>>(model.SeasonTimeModel),
                        SpecialDayModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Commands.PlaceCalendar.SpecialDayViewModel>>(model.SpecialDayModel),
                    });
                    return new PlaceCalendarResponseViewModel
                    {
                        Success = true
                    };
                }
                catch (Exception e)
                {
                    return new PlaceCalendarResponseViewModel { };
                }
            }
            else
            {
                return new PlaceCalendarResponseViewModel { };
            }
        }
    }
}
