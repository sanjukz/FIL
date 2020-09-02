using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Events;
using FIL.Contracts.QueryResults.Events;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class ExternalEventQueryHandler : IQueryHandler<ExternalEventQuery, ExternalEventQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public ExternalEventQueryHandler(IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, FIL.Logging.ILogger logger)
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _logger = logger;
        }

        public ExternalEventQueryResult Handle(ExternalEventQuery query)
        {
            try
            {
                var events = AutoMapper.Mapper.Map<Contracts.Models.Event>(_eventRepository.GetByAltId(query.EventAltId));
                if (events != null)
                {
                    var eventDetails = _eventDetailRepository.GetSubEventByEventId(events.Id).ToList();
                    var eventName = events.Name;
                    var eventAltid = events.AltId;
                    var startDateTime = eventDetails.Min(s => s.StartDateTime);
                    var endDateTime = eventDetails.Max(s => s.StartDateTime);

                    var eventDetailData = eventDetails.Select(ed =>
                    {
                        return new EventDetails
                        {
                            Name = ed.Name,
                            AltId = ed.AltId,
                            StartDateTime = ed.StartDateTime,
                            EndDateTime = ed.EndDateTime
                        };
                    });

                    var eventContainer = new EventContainer
                    {
                        Name = eventName,
                        AltId = eventAltid,
                        StartDateTime = startDateTime,
                        EndDateTime = endDateTime,
                        EventDetails = eventDetailData.ToList(),
                    };

                    return new ExternalEventQueryResult
                    {
                        EventContainer = eventContainer,
                        IsValid = true
                    };
                }
                else
                {
                    return new ExternalEventQueryResult
                    {
                        EventContainer = null,
                        IsValid = false,
                        ErrorMessage = "event not active"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ExternalEventQueryResult
                {
                    EventContainer = null,
                    IsValid = false,
                    ErrorMessage = ex.ToString()
                };
            }
        }
    }
}