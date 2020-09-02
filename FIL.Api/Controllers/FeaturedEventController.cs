using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers
{
    public class FeaturedEventController : Controller
    {
        private readonly IFeaturedEventRepository _featuredEventRepository;
        private readonly IEventRepository _eventRepository;

        public FeaturedEventController(IFeaturedEventRepository featuredEventRepository, IEventRepository eventRepository)
        {
            _featuredEventRepository = featuredEventRepository;
            _eventRepository = eventRepository;
        }

        [HttpGet]
        [Route("api/featuredevent/all")]
        public IEnumerable<FeaturedEvent> GetAll()
        {
            return _featuredEventRepository.GetAll();
        }

        [HttpGet]
        [Route("api/featuredevent/get/{id}")]
        public FeaturedEvent Get(int id)
        {
            return _featuredEventRepository.Get(id);
        }

        [HttpPost]
        [Route("api/featuredevent/save")]
        public FeaturedEvent Save([FromBody] FeaturedEvent featuredEvent)
        {
            return _featuredEventRepository.Save(featuredEvent);
        }

        [HttpPost]
        [Route("api/featuredevent/delete")]
        public void Delete([FromBody] FeaturedEvent featuredEvent)
        {
            _featuredEventRepository.Delete(featuredEvent);
        }
    }
}