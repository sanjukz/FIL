using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers
{
    public class EventItemController : Controller
    {
        private readonly IEventItemRepository _eventItemRepository;

        public EventItemController(IEventItemRepository eventItemRepository)
        {
            _eventItemRepository = eventItemRepository;
        }

        [HttpGet]
        [Route("api/eventitem/all")]
        public IEnumerable<EventItem> GetAll()
        {
            return _eventItemRepository.GetAll();
        }

        [HttpGet]
        [Route("api/eventitem/get/{id}")]
        public EventItem Get(int id)
        {
            return _eventItemRepository.Get(id);
        }

        [HttpPost]
        [Route("api/eventitem/save")]
        public EventItem Save([FromBody] EventItem objEvent)
        {
            return _eventItemRepository.Save(objEvent);
        }

        [HttpPost]
        [Route("api/eventitem/delete")]
        public void Delete([FromBody] EventItem objEvent)
        {
            _eventItemRepository.Delete(objEvent);
        }
    }
}