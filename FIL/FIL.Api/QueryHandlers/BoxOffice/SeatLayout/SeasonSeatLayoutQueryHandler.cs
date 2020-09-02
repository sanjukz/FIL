using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice.SeatLayout;
using FIL.Contracts.QueryResults.Boxoffice.SeatLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice.SeatLayout
{
    public class SeasonSeatLayoutQueryHandler : IQueryHandler<SeasonSeatLayoutQuery, SeasonSeatLayoutQueryResult>
    {
        private readonly ISeatLayoutRepository _seatLayoutRepository;
        private readonly Logging.ILogger _logger;

        public SeasonSeatLayoutQueryHandler(ISeatLayoutRepository seatLayoutRepository, Logging.ILogger logger, IWheelchairSeatMappingRepository wheelchairSeatMappingRepository)
        {
            _seatLayoutRepository = seatLayoutRepository;
            _logger = logger;
        }

        public SeasonSeatLayoutQueryResult Handle(SeasonSeatLayoutQuery query)
        {
            try
            {
                SeatLayoutData matchSeatTicketDetailList = AutoMapper.Mapper.Map<SeatLayoutData>(_seatLayoutRepository.GetSeasonSeatLayoutData(query.EventTicketAttributeId));
                var orderByRowColumnOrder = matchSeatTicketDetailList.SeatLayouts.OrderByDescending(g => g.RowOrder).ThenBy(g => g.ColumnOrder).ToList();
                List<SeatItem> seatItems = AutoMapper.Mapper.Map<List<SeatItem>>(orderByRowColumnOrder);
                List<Row> Rows = new List<FIL.Contracts.Models.Row>();
                var groupByRowNumber = seatItems.GroupBy(g => g.RowNumber);
                foreach (var item in groupByRowNumber)
                {
                    var seat = seatItems.Where(s => s.RowNumber == item.Key.ToString()).ToList();
                    FIL.Contracts.Models.Row Row = new FIL.Contracts.Models.Row();
                    Row.SeatItems = seat;
                    Rows.Add(Row);
                }
                return new SeasonSeatLayoutQueryResult
                {
                    Rows = Rows,
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
            return new SeasonSeatLayoutQueryResult
            {
                Rows = null
            };
        }
    }
}