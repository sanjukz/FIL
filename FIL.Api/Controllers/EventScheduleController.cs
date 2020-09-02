using FIL.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FIL.Api.Controllers
{
    public class EventScheduleController : Controller
    {
        private readonly IEventScheduleRepository _eventScheduleRepository;

        public EventScheduleController(IEventScheduleRepository eventScheduleRepository)
        {
            _eventScheduleRepository = eventScheduleRepository;
        }

        public class EventSchedule
        {
            public List<FIL.Contracts.DataModels.EventSchedule> EventSchedules { get; set; }
        }

        [HttpGet]
        [Route("api/save/eventSchedule/{count}")]
        public IEnumerable<FIL.Contracts.DataModels.EventSchedule> GetAll(int count)
        {
            var schedules = new List<FIL.Contracts.DataModels.EventSchedule>();
            for (var i = 0; i < count; i++)
            {
                var schedule = new FIL.Contracts.DataModels.EventSchedule();
                schedule.EventFrequencyTypeId = Contracts.Enums.EventFrequencyType.Recurring;
                schedule.OccuranceTypeId = Contracts.Enums.OccuranceType.Weekly;
                schedule.Name = "Jan";
                schedule.StartDateTime = DateTime.Now;
                schedule.EndDateTime = DateTime.Now;
                schedule.DayId = "1,2,3";
                schedule.EventId = 25;
                schedule.IsEnabled = true;
                schedule.CreatedBy = System.Guid.NewGuid();
                schedule.UpdatedBy = System.Guid.NewGuid();
                schedule.CreatedUtc = DateTime.UtcNow;
                schedules.Add(schedule);
            }
            var eventSchedules = _eventScheduleRepository.SaveAll(schedules);
            return eventSchedules;
        }
    }
}