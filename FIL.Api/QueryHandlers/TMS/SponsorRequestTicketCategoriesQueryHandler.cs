using FIL.Api.Repositories;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResults.TMS;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.TMS
{
    public class SponsorRequestTicketCategoriesQueryHandler : IQueryHandler<SponsorRequestTicketCategoriesQuery, sponsorRequestTicketCategoriesQueryResult>
    {
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public SponsorRequestTicketCategoriesQueryHandler(
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventDetailRepository eventDetailRepository)
        {
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        public sponsorRequestTicketCategoriesQueryResult Handle(SponsorRequestTicketCategoriesQuery query)
        {
            var eventDetail = _eventDetailRepository.GetByAltId(query.EventDetailAltId);
            var eventTicketDetails = _eventTicketDetailRepository.GetSponsorRequestTicketCategories(eventDetail.Id);

            return new sponsorRequestTicketCategoriesQueryResult
            {
                SponsorRequestTicketCategories = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TMS.SponsorRequestTicketCategory>>(eventTicketDetails)
            };
        }
    }
}