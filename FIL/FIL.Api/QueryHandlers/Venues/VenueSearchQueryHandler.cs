using FIL.Api.Repositories;
using FIL.Contracts.Queries.Venue;
using FIL.Contracts.QueryResults;

namespace FIL.Api.QueryHandlers.Venues
{
    public class VenueSearchQueryHandler : IQueryHandler<VenueSearchQuery, VenueSearchQueryResult>
    {
        private readonly IVenueRepository _venueRepository;

        public VenueSearchQueryHandler(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        public VenueSearchQueryResult Handle(VenueSearchQuery query)
        {
            var venueResult = _venueRepository.GetByName(query.Name);
            return new VenueSearchQueryResult
            {
                IsExisting = venueResult != null,
            };
        }
    }
}