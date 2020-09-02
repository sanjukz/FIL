using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.TournamentLayout;
using FIL.Contracts.QueryResults.TournamentLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TournamentLayouts
{
    public class TournamentLayoutViewSeatQueryHandler : IQueryHandler<TournamentLayoutViewSeatLayoutQuery, TournamentLayoutViewSeatLayoutQueryResult>
    {
        private ITournamentLayoutSectionRepository _tournamentLayoutSectionRepository;
        private readonly ITournamentLayoutSectionSeatRepository _tournamentLayoutSectionSeatRepository;

        public TournamentLayoutViewSeatQueryHandler(ITournamentLayoutSectionRepository tournamentLayoutSectionRepository, ITournamentLayoutSectionSeatRepository tournamentLayoutSectionSeatRepository)
        {
            _tournamentLayoutSectionRepository = tournamentLayoutSectionRepository;
            _tournamentLayoutSectionSeatRepository = tournamentLayoutSectionSeatRepository;
        }

        public TournamentLayoutViewSeatLayoutQueryResult Handle(TournamentLayoutViewSeatLayoutQuery query)
        {
            try
            {
                List<Contracts.Models.TournamentLayoutSectionSeat> TournamentLayoutSectionSeatList = new List<Contracts.Models.TournamentLayoutSectionSeat>();
                var TournamentLayoutSection = _tournamentLayoutSectionRepository.Get(query.Id);
                var seatLayouCount = _tournamentLayoutSectionSeatRepository.GetSeatCount
                        (TournamentLayoutSection.Id);

                var isSeatLayout = seatLayouCount != 0 ? true : false;
                List<Contracts.DataModels.TournamentLayoutSectionSeat> TournamentLayoutSectionSeat = AutoMapper.Mapper.Map<List<Contracts.DataModels.TournamentLayoutSectionSeat>>(_tournamentLayoutSectionSeatRepository.GetByTournamentLayoutSectionId(TournamentLayoutSection.Id));

                for (int k = 0; k < TournamentLayoutSectionSeat.Count; k = k + 2000)
                {
                    var matchSeatTicketDetailListBatcher = TournamentLayoutSectionSeat.Skip(k).Take(2000);

                    var MatchLayoutSectionSeatIds = matchSeatTicketDetailListBatcher.Select(s => s.TournamentLayoutSectionId).FirstOrDefault();

                    List<Contracts.DataModels.TournamentLayoutSectionSeat> matchLayoutSectionSeats = AutoMapper.Mapper.Map<List<Contracts.DataModels.TournamentLayoutSectionSeat>>(_tournamentLayoutSectionSeatRepository.GetByTournamentLayoutSectionId(MatchLayoutSectionSeatIds));

                    List<Contracts.Models.TournamentLayoutSectionSeat> MatchLayoutSectionSeats = AutoMapper.Mapper.Map<List<Contracts.Models.TournamentLayoutSectionSeat>>(matchLayoutSectionSeats);

                    foreach (var item in MatchLayoutSectionSeats)
                    {
                        TournamentLayoutSectionSeatList.Add(item);
                    }
                }
                var orderByRowColumnOrder = TournamentLayoutSectionSeatList.OrderByDescending(g => g.RowOrder).ThenBy(g => g.ColumnOrder).ToList();
                List<FIL.Contracts.Models.TournamentLayouts.MasterVenueSeatItem> seatItems = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TournamentLayouts.MasterVenueSeatItem>>(orderByRowColumnOrder);
                List<FIL.Contracts.Models.TournamentLayouts.TournamentLayoutSectionSeatModel> Rows = new List<FIL.Contracts.Models.TournamentLayouts.TournamentLayoutSectionSeatModel>();
                var groupByRowNumber = seatItems.GroupBy(g => g.RowNumber);

                foreach (var item in groupByRowNumber)
                {
                    var seat = seatItems.Where(s => s.RowNumber == item.Key.ToString()).ToList();
                    FIL.Contracts.Models.TournamentLayouts.TournamentLayoutSectionSeatModel Row = new FIL.Contracts.Models.TournamentLayouts.TournamentLayoutSectionSeatModel();
                    Row.MasterVenueSeatItems = seat;
                    Rows.Add(Row);
                }
                return new TournamentLayoutViewSeatLayoutQueryResult
                {
                    MasterVenueRows = Rows,
                    IsSeatLayout = isSeatLayout
                };
            }
            catch (Exception ex)
            {
            }

            return new TournamentLayoutViewSeatLayoutQueryResult
            {
                MasterVenueRows = null,
            };
        }
    }
}