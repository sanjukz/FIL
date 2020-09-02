using FIL.Api.Repositories;
using FIL.Contracts.Queries.PublishEvent;
using FIL.Contracts.QueryResults.PublishEvent;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.Events
{
    public class PublishSubEventQueryHandler : IQueryHandler<PublishSubEventQuery, PublishSubEventQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IFeaturedEventRepository _featuredEventRepository;

        public PublishSubEventQueryHandler(IEventDetailRepository eventDetailRepository, IFeaturedEventRepository featuredEventRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _featuredEventRepository = featuredEventRepository;
        }

        public PublishSubEventQueryResult Handle(PublishSubEventQuery query)
        {
            var eventDetailDataModel = _eventDetailRepository.GetByEvent(query.EventId);
            var eventDetailModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventDetail>>(eventDetailDataModel);

            var featuredEventDataModel = _featuredEventRepository.GetByEventId(query.EventId);
            var featuredEventModel = AutoMapper.Mapper.Map<Contracts.Models.FeaturedEvent>(featuredEventDataModel);

            var siteId = Enum.GetValues(typeof(Contracts.Enums.Site));
            List<string> siteList = new List<string>();

            foreach (var item in siteId)
            {
                siteList.Add(item.ToString());
            }

            return new PublishSubEventQueryResult
            {
                SubEvents = eventDetailModel,
                featuredEvents = featuredEventModel,
                Sites = siteList
            };
        }
    }
}