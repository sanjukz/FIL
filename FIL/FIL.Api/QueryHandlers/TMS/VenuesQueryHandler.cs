using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TMS;
using FIL.Contracts.QueryResult.TMS;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TMS
{
    public class VenuesQueryHandler : IQueryHandler<VenueQuery, VenueQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserVenueMappingRepository _userVenueMappingRepository;
        private readonly IVenueRepository _venueRepository;

        public VenuesQueryHandler(IUserRepository userRepository, IUserVenueMappingRepository userVenueMappingRepository, IVenueRepository venueRepository)
        {
            _userRepository = userRepository;
            _userVenueMappingRepository = userVenueMappingRepository;
            _venueRepository = venueRepository;
        }

        public VenueQueryResult Handle(VenueQuery query)
        {
            var userId = _userRepository.GetByAltId(query.UserAltId).Id;
            var userVenue = _userVenueMappingRepository.GetByUserId(userId);
            var venues = _venueRepository.GetByVenueIds(userVenue.Select(s => s.VenueId).Distinct());
            return new VenueQueryResult
            {
                venues = AutoMapper.Mapper.Map<List<Venue>>(venues)
            };
        }
    }
}