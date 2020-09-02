using FIL.Api.Repositories;
using FIL.Contracts.Queries.MatchLayout;
using FIL.Contracts.QueryResults.MatchLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.MatchLayout
{
    public class MatchLevelViewSeatQueryHandler : IQueryHandler<MatchLevelViewSeatQuery, MatchLevelViewSeatQueryResult>
    {
        private readonly IMatchLayoutSectionRepository _matchLayoutSectionRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;

        public MatchLevelViewSeatQueryHandler(IMatchLayoutSectionRepository matchLayoutSectionRepository, IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository)
        {
            _matchLayoutSectionRepository = matchLayoutSectionRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
        }

        public MatchLevelViewSeatQueryResult Handle(MatchLevelViewSeatQuery query)
        {
            try
            {
                List<Contracts.Models.MatchLayoutSectionSeat> MatchLayoutSectionSeatList = new List<Contracts.Models.MatchLayoutSectionSeat>();
                var matchLayoutSection = _matchLayoutSectionRepository.Get(query.Id);
                var seatLayouCount = _matchLayoutSectionSeatRepository.GetSeatCount
                        (matchLayoutSection.Id);
                var isSeatLayout = seatLayouCount != 0 ? true : false;
                List<Contracts.DataModels.MatchLayoutSectionSeat> matchLayoutSectionSeat = AutoMapper.Mapper.Map<List<Contracts.DataModels.MatchLayoutSectionSeat>>(_matchLayoutSectionSeatRepository.GetByMatchLayoutSectionId(matchLayoutSection.Id));
                for (int k = 0; k < matchLayoutSectionSeat.Count; k = k + 2000)
                {
                    var matchSeatTicketDetailListBatcher = matchLayoutSectionSeat.Skip(k).Take(2000);

                    var MatchLayoutSectionSeatIds = matchSeatTicketDetailListBatcher.Select(s => s.MatchLayoutSectionId).FirstOrDefault();

                    List<Contracts.DataModels.MatchLayoutSectionSeat> matchLayoutSectionSeats = AutoMapper.Mapper.Map<List<Contracts.DataModels.MatchLayoutSectionSeat>>(_matchLayoutSectionSeatRepository.GetByMatchLayoutSectionId(MatchLayoutSectionSeatIds));

                    List<Contracts.Models.MatchLayoutSectionSeat> MatchLayoutSectionSeats = AutoMapper.Mapper.Map<List<Contracts.Models.MatchLayoutSectionSeat>>(matchLayoutSectionSeats);

                    foreach (var item in MatchLayoutSectionSeats)
                    {
                        MatchLayoutSectionSeatList.Add(item);
                    }
                }
                var orderByRowColumnOrder = MatchLayoutSectionSeatList.OrderByDescending(g => g.RowOrder).ThenBy(g => g.ColumnOrder).ToList();
                List<FIL.Contracts.Models.MatchLevel.MatchLevelSeatItem> seatItems = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.MatchLevel.MatchLevelSeatItem>>(orderByRowColumnOrder);
                List<FIL.Contracts.Models.MatchLevel.MatchLayoutSeatModel> Rows = new List<FIL.Contracts.Models.MatchLevel.MatchLayoutSeatModel>();
                var groupByRowNumber = seatItems.GroupBy(g => g.RowNumber);
                foreach (var item in groupByRowNumber)
                {
                    var seat = seatItems.Where(s => s.RowNumber == item.Key.ToString()).ToList();
                    FIL.Contracts.Models.MatchLevel.MatchLayoutSeatModel Row = new FIL.Contracts.Models.MatchLevel.MatchLayoutSeatModel();
                    Row.MasterVenueSeatItems = seat;
                    Rows.Add(Row);
                }
                return new MatchLevelViewSeatQueryResult
                {
                    MatchLevelRows = Rows,
                    IsSeatLayout = isSeatLayout
                };
            }
            catch (Exception ex)
            {
                return new MatchLevelViewSeatQueryResult
                {
                    MatchLevelRows = null,
                    IsSeatLayout = false
                };
            }
        }
    }
}