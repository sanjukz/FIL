using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.BoxOffice.SeatLayout;
using FIL.Contracts.QueryResults.BoxOffice.SeatLayout;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.BoxOffice.SeatLayout
{
    public class SeasonSeatInfoQueryHandler : IQueryHandler<SeasonSeatQuery, SeasonSeatQueryResult>
    {
        private readonly ISeasonSeatRepository _seasonSeatRepository;
        private readonly Logging.ILogger _logger;

        public SeasonSeatInfoQueryHandler(ISeasonSeatRepository seasonSeatRepository, Logging.ILogger logger)
        {
            _seasonSeatRepository = seasonSeatRepository;
            _logger = logger;
        }

        public SeasonSeatQueryResult Handle(SeasonSeatQuery query)
        {
            try
            {
                SeasonSeatInfo seasonSeatInfo = AutoMapper.Mapper.Map<SeasonSeatInfo>(_seasonSeatRepository.GetSeasonSeatInfo(query.EventTicketAttributeId, query.SeatTag));

                return new SeasonSeatQueryResult
                {
                    SeasonSeat = AutoMapper.Mapper.Map<List<SeasonSeat>>(seasonSeatInfo.SeasonSeats)
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
            return new SeasonSeatQueryResult
            {
                SeasonSeat = null
            };
        }
    }
}