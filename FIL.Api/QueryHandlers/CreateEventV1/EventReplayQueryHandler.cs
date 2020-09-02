using FIL.Api.Repositories;
using FIL.Contracts.Models.CreateEventV1;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class EventReplayQueryHandler : IQueryHandler<EventReplayQuery, EventReplayQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IReplayDetailRepository _replayDetailRepository;

        public EventReplayQueryHandler(IEventRepository eventRepository,
            IReplayDetailRepository replayDetailRepository)
        {
            _eventRepository = eventRepository;
            _replayDetailRepository = replayDetailRepository;
        }

        public EventReplayQueryResult Handle(EventReplayQuery query)
        {
            try
            {
                var currentEvent = _eventRepository.Get(query.EventId);
                var replayDetails = _replayDetailRepository.GetAllByEventId(query.EventId);
                List<FIL.Contracts.Models.CreateEventV1.ReplayDetailModel> replayDetails1 = new List<FIL.Contracts.Models.CreateEventV1.ReplayDetailModel>();
                foreach (var replayDetail in replayDetails)
                {
                    ReplayDetailModel replayDetail1 = new ReplayDetailModel();
                    replayDetail1.StartDate = replayDetail.StartDate;
                    replayDetail1.EndDate = replayDetail.EndDate;
                    replayDetail1.IsEnabled = replayDetail.IsEnabled;
                    replayDetail1.IsPaid = replayDetail.IsPaid;
                    replayDetail1.Price = replayDetail.Price;
                    replayDetail1.CurrencyId = replayDetail.CurrencyId;
                    replayDetails1.Add(replayDetail1);
                }
                return new EventReplayQueryResult
                {
                    Success = true,
                    EventId = query.EventId,
                    ReplayDetailModel = replayDetails1
                };
            }
            catch (Exception e)
            {
                return new EventReplayQueryResult { };
            }
        }
    }
}