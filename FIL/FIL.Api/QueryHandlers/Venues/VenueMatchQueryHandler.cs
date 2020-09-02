using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Venue;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Venues
{
    public class VenueMatchQueryHandler : IQueryHandler<VenueMatchQuery, VenueMatchQueryResult>
    {
        private readonly IVenueRepository _venueRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IUserVenueMappingRepository _userVenueMappingRepository;

        public VenueMatchQueryHandler(IUserVenueMappingRepository userVenueMappingRepository, IVenueRepository venueRepository, FIL.Logging.ILogger logger)
        {
            _venueRepository = venueRepository;
            _logger = logger;
            _userVenueMappingRepository = userVenueMappingRepository;
        }

        public VenueMatchQueryResult Handle(VenueMatchQuery query)
        {
            try
            {
                var venues = _venueRepository.GetAll(null).OrderByDescending(s => s.CreatedUtc);

                return new VenueMatchQueryResult
                {
                    Venues = AutoMapper.Mapper.Map<List<Venue>>(venues).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new VenueMatchQueryResult
                {
                    Venues = null
                };
            }
        }
    }
}