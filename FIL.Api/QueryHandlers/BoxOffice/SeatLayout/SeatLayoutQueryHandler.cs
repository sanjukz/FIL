using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice.SeatLayout;
using FIL.Contracts.QueryResults.Boxoffice.SeatLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice.SeatLayout
{
    public class SeatLayoutQueryHandler : IQueryHandler<SeatLayoutQuery, SeatLayoutQueryResult>
    {
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IMatchLayoutCompanionSeatMappingRepository _matchLayoutCompanionSeatMappingRepository;

        public SeatLayoutQueryHandler(IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository
           , IMatchLayoutCompanionSeatMappingRepository matchLayoutCompanionSeatMappingRepository
            )
        {
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _matchLayoutCompanionSeatMappingRepository = matchLayoutCompanionSeatMappingRepository;
        }

        public SeatLayoutQueryResult Handle(SeatLayoutQuery query)
        {
            try
            {
                if (query.Channels == Channels.Corporate && query.AllocationType == AllocationType.Venue)
                {
                    var eventTicketDetails = _eventTicketDetailRepository.GetAllByTicketCategoryIdAndEventDetailId((long)query.TicketCategoryId, (long)query.EventDetailId);
                    List<Contracts.DataModels.MatchLayoutSectionSeat> MatchLayoutSectionSeatList = AutoMapper.Mapper.Map<List<Contracts.DataModels.MatchLayoutSectionSeat>>(_matchLayoutSectionSeatRepository.GetByEventTicketDetailId(eventTicketDetails.Id));
                    var orderByRowColumnOrder = MatchLayoutSectionSeatList.OrderByDescending(g => g.RowOrder).OrderBy(g => g.ColumnOrder).ToList();
                    List<SeatItem> seatItems = AutoMapper.Mapper.Map<List<SeatItem>>(orderByRowColumnOrder);
                    List<Row> Rows = new List<Row>();
                    var groupByRowNumber = seatItems.GroupBy(g => g.RowNumber);

                    foreach (var item in groupByRowNumber)
                    {
                        var seat = seatItems.Where(s => s.RowNumber == item.Key.ToString()).ToList();
                        Row Row = new Row();
                        Row.SeatItems = seat;
                        Rows.Add(Row);
                    }
                    var matchLayoutCompanionSeats = _matchLayoutCompanionSeatMappingRepository.GetByWheelChairSeatIds(MatchLayoutSectionSeatList.Where(w => w.SeatTypeId == SeatType.WheelChair).Select(s => s.Id));
                    return new SeatLayoutQueryResult
                    {
                        Rows = Rows,
                        MatchLayoutCompanionSeatMappings = matchLayoutCompanionSeats.ToList()
                    };
                }
                else
                {
                    var eventTicketAttribute = _eventTicketAttributeRepository.Get((long)query.EventTicketAttributeId);
                    List<Contracts.DataModels.MatchLayoutSectionSeat> MatchLayoutSectionSeatList = AutoMapper.Mapper.Map<List<Contracts.DataModels.MatchLayoutSectionSeat>>(_matchLayoutSectionSeatRepository.GetByEventTicketDetailId(eventTicketAttribute.EventTicketDetailId));
                    var orderByRowColumnOrder = MatchLayoutSectionSeatList.OrderByDescending(g => g.RowOrder).OrderBy(g => g.ColumnOrder).ToList();
                    List<SeatItem> seatItems = AutoMapper.Mapper.Map<List<SeatItem>>(orderByRowColumnOrder);
                    List<Row> Rows = new List<Row>();
                    var groupByRowNumber = seatItems.GroupBy(g => g.RowNumber);

                    foreach (var item in groupByRowNumber)
                    {
                        var seat = seatItems.Where(s => s.RowNumber == item.Key.ToString()).ToList();
                        Row Row = new Row();
                        Row.SeatItems = seat;
                        Rows.Add(Row);
                    }
                    var matchLayoutCompanionSeats = _matchLayoutCompanionSeatMappingRepository.GetByWheelChairSeatIds(MatchLayoutSectionSeatList.Where(w => w.SeatTypeId == SeatType.WheelChair).Select(s => s.Id));
                    return new SeatLayoutQueryResult
                    {
                        Rows = Rows,
                        MatchLayoutCompanionSeatMappings = matchLayoutCompanionSeats.ToList()
                    };
                }
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