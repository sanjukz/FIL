using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.SeatLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.SeatLayout
{
    public class SeatLayoutQueryHandler : IQueryHandler<SeatLayoutQuery, SeatLayoutQueryResult>
    {
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IMatchLayoutCompanionSeatMappingRepository _matchLayoutCompanionSeatMappingRepository;

        public SeatLayoutQueryHandler(IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository, IEventTicketAttributeRepository eventTicketAttributeRepository
             , IMatchLayoutCompanionSeatMappingRepository matchLayoutCompanionSeatMappingRepository)
        {
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _matchLayoutCompanionSeatMappingRepository = matchLayoutCompanionSeatMappingRepository;
        }
            
        public SeatLayoutQueryResult Handle(SeatLayoutQuery query)
        {
            try
            {
                var eventTicketAttribute = _eventTicketAttributeRepository.Get(query.EventTicketAttributeId);
                List<Contracts.DataModels.MatchLayoutSectionSeat> MatchLayoutSectionSeatList = AutoMapper.Mapper.Map<List<Contracts.DataModels.MatchLayoutSectionSeat>>(_matchLayoutSectionSeatRepository.GetByEventTicketDetailId(eventTicketAttribute.EventTicketDetailId));
                var orderByRowColumnOrder = MatchLayoutSectionSeatList.OrderByDescending(g => g.RowOrder).OrderBy(g => g.ColumnOrder).ToList();
                List<FIL.Contracts.Models.SeatItem> seatItems = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.SeatItem>>(orderByRowColumnOrder);
                List<FIL.Contracts.Models.Row> Rows = new List<FIL.Contracts.Models.Row>();
                var groupByRowNumber = seatItems.GroupBy(g => g.RowNumber);

                foreach (var item in groupByRowNumber)
                {
                    var seat = seatItems.Where(s => s.RowNumber == item.Key.ToString()).ToList();
                    FIL.Contracts.Models.Row Row = new FIL.Contracts.Models.Row();
                    Row.SeatItems = seat;
                    Rows.Add(Row);
                }

                var matchLayoutCompanionSeats = _matchLayoutCompanionSeatMappingRepository.GetByWheelChairSeatIds(MatchLayoutSectionSeatList.Where(w => w.SeatTypeId == Contracts.Enums.SeatType.WheelChair).Select(s => s.Id));

                return new SeatLayoutQueryResult
                {
                    Rows = Rows,
                    MatchLayoutCompanionSeatMappings = matchLayoutCompanionSeats.ToList()
                };
            }
            catch (Exception ex)
            {
            }
            return new SeatLayoutQueryResult
            {
                Rows = null
            };
        }
    }
}