using FIL.Api.Repositories;
using FIL.Contracts.Queries.VenueCreation;
using FIL.Contracts.QueryResults.VenueCreation;

namespace FIL.Api.QueryHandlers.VenueCreation
{
    public class VenueDataQueryHandler : IQueryHandler<VenueDataQuery, VenueDataQueryResult>
    {
        private readonly IVenueRepository _venueRepository;

        public VenueDataQueryHandler(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        public VenueDataQueryResult Handle(VenueDataQuery query)
        {
            var venueDataModel = _venueRepository.GetByAltId(query.VenueAltId);
            var venueModel = AutoMapper.Mapper.Map<Contracts.Models.Venue>(venueDataModel);
            return new VenueDataQueryResult
            {
                Venues = venueModel
            };
        }
    }
}