using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        [Route("api/event/all")]
        public IEnumerable<Event> GetAll()
        {
            return _eventRepository.GetAll();
        }

        [HttpGet]
        [Route("api/event/get/{id}")]
        public Event Get(int id)
        {
            return _eventRepository.Get(id);
        }

        [HttpPost]
        [Route("api/event/save")]
        public Event Save([FromBody] Event objEvent)
        {
            return _eventRepository.Save(objEvent);
        }

        [HttpPost]
        [Route("api/event/delete")]
        public void Delete([FromBody] Event objEvent)
        {
            _eventRepository.Delete(objEvent);
        }
    }
}