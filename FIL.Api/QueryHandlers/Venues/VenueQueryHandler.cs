using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Venue;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Venues
{
    public class VenueQueryHandler : IQueryHandler<VenueQuery, VenueQueryResult>
    {
        private readonly IVenueRepository _venueRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IUserVenueMappingRepository _userVenueMappingRepository;

        public VenueQueryHandler(IUserVenueMappingRepository userVenueMappingRepository, IVenueRepository venueRepository, FIL.Logging.ILogger logger)
        {
            _venueRepository = venueRepository;
            _logger = logger;
            _userVenueMappingRepository = userVenueMappingRepository;
        }

        public VenueQueryResult Handle(VenueQuery query)
        {
            try
            {
                var venues = _venueRepository.GetAllVenues();
                return new VenueQueryResult
                {
                    Venues = AutoMapper.Mapper.Map<List<Venue>>(venues).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new VenueQueryResult
                {
                    Venues = null
                };
            }
        }
    }
}