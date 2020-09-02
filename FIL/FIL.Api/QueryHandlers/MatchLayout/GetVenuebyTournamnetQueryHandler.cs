using FIL.Api.Repositories;
using FIL.Contracts.Queries.MatchLayout;
using FIL.Contracts.QueryResults.MatchLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.MatchLayout
{
    public class GetVenuebyTournamnetQueryHandler : IQueryHandler<GetTornamentVenueQuery, GetTournamentVenueQueryResult>
    {
        private readonly IVenueRepository _venueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IEventFeeTypeMappingRepository _eventFeeTypeMappingRepository;

        public GetVenuebyTournamnetQueryHandler(IEventDetailRepository eventDetailRepository, IVenueRepository venueRepository, FIL.Logging.ILogger logger, IEventFeeTypeMappingRepository eventFeeTypeMappingRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _logger = logger;
            _eventFeeTypeMappingRepository = eventFeeTypeMappingRepository;
        }

        public GetTournamentVenueQueryResult Handle(GetTornamentVenueQuery query)
        {
            try
            {
                var eventdetails = _eventDetailRepository.GetAllByEventId(query.eventid);
                var venues = _venueRepository.GetByVenueIds(eventdetails.Select(s => s.VenueId));
                var feeTypeMapping = _eventFeeTypeMappingRepository.GetByEventId(query.eventid);

                List<string> channelList = new List<string>();
                channelList.AddRange(Enum.GetNames(typeof(FIL.Contracts.Enums.Channels)));
                List<string> feeType = new List<string>();
                feeType.AddRange(Enum.GetNames(typeof(FIL.Contracts.Enums.FeeType)));
                List<string> valueType = new List<string>();
                valueType.AddRange(Enum.GetNames(typeof(FIL.Contracts.Enums.ValueTypes)));

                return new GetTournamentVenueQueryResult
                {
                    venues = AutoMapper.Mapper.Map<List<Contracts.Models.Venue>>(venues).ToList(),
                    feeTypeMapping = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.EventFeeTypeMapping>>(feeTypeMapping.ToList()),
                    channels = channelList,
                    feeType = feeType,
                    valueType = valueType,
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new GetTournamentVenueQueryResult
                {
                    venues = null
                };
            }
        }
    }
}