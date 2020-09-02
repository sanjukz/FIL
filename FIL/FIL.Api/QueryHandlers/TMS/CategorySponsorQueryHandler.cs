using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.TMS;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResults.TMS;
using System;

namespace FIL.Api.QueryHandlers.TMS
{
    public class CategorySponsorQueryHandler : IQueryHandler<CategorySponsorQuery, CategorySponsorQueryResult>
    {
        private readonly IVenueRepository _venuetRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ICategorySponsorDetailRepository _categorySponsorDetailRepository;

        public CategorySponsorQueryHandler(
            IVenueRepository venuetRepository,
            IEventRepository eventRepository,
            ICategorySponsorDetailRepository categorySponsorDetailRepository)
        {
            _venuetRepository = venuetRepository;
            _eventRepository = eventRepository;
            _categorySponsorDetailRepository = categorySponsorDetailRepository;
        }

        public CategorySponsorQueryResult Handle(CategorySponsorQuery query)
        {
            CategorySponsorDataModel categorySponsorDataModel = new CategorySponsorDataModel();
            if (query.AllocationType == AllocationType.Match)
            {
                categorySponsorDataModel = _categorySponsorDetailRepository.GetSponsorCategoryData((long)query.EventTicketAttributeId);
            }
            if (query.AllocationType == AllocationType.Venue)
            {
                var venueId = _venuetRepository.GetByAltId((Guid)query.VenueAltId).Id;
                var eventId = _eventRepository.GetByAltId((Guid)query.EventAltId).Id;
                categorySponsorDataModel = _categorySponsorDetailRepository.GetVenueCategorySponsorDetail(eventId, venueId, (long)query.TicketCategoryId);
            }
            return new CategorySponsorQueryResult
            {
                CategorySponsorDataModel = categorySponsorDataModel
            };
        }
    }
}