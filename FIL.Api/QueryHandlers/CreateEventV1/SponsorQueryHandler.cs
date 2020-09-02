using FIL.Api.Repositories;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class SponsorQueryHandler : IQueryHandler<SponsorQuery, SponsorQueryResult>
    {
        private readonly IFILSponsorDetailRepository _fILSponsorDetailRepository;
        private readonly IEventRepository _eventRepository;

        public SponsorQueryHandler(
           IFILSponsorDetailRepository fILSponsorDetailRepository,
            IEventRepository eventRepository
            )
        {
            _fILSponsorDetailRepository = fILSponsorDetailRepository;
            _eventRepository = eventRepository;
        }

        public SponsorQueryResult Handle(SponsorQuery query)
        {
            try
            {
                var eventData = _eventRepository.Get(query.EventId);
                if (eventData == null)
                {
                    return new SponsorQueryResult { Success = true };
                }
                var sponsorDetails = _fILSponsorDetailRepository.GetAllByEventId(query.EventId).ToList();
                return new SponsorQueryResult
                {
                    Success = true,
                    EventId = query.EventId,
                    IsValidLink = true,
                    IsDraft = false,
                    SponsorDetails = sponsorDetails
                };
            }
            catch (Exception e)
            {
                return new SponsorQueryResult { };
            }
        }
    }
}