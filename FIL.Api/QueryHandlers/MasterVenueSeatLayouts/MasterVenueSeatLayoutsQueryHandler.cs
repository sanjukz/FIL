using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterVenueViewSeat;
using FIL.Contracts.QueryResults.MasterVenueViewSeat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.MasterVenueSeatLayouts
{
    public class MasterVenueSeatLayoutsQueryHandler : IQueryHandler<MasterVenueViewSeatLayoutQuery, MasterVenueViewSeatLayoutQueryResult>
    {
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IMasterVenueLayoutSectionSeatRepository _masterVenueLayoutSectionSeatRepository;

        public MasterVenueSeatLayoutsQueryHandler(IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository, IMasterVenueLayoutSectionSeatRepository masterVenueLayoutSectionSeatRepository)
        {
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _masterVenueLayoutSectionSeatRepository = masterVenueLayoutSectionSeatRepository;
        }

        public MasterVenueViewSeatLayoutQueryResult Handle(MasterVenueViewSeatLayoutQuery query)
        {
            try
            {
                List<Contracts.Models.MasterVenueLayoutSectionSeat> MasterVenueLayoutSectionSeatList = new List<Contracts.Models.MasterVenueLayoutSectionSeat>();
                var masterVenueLayoutSection = _masterVenueLayoutSectionRepository.Get(query.Id);

                var seatLayouCount = _masterVenueLayoutSectionSeatRepository.GetSeatCount
                    (masterVenueLayoutSection.Id);

                var isSeatLayout = seatLayouCount != 0 ? true : false;
                List<Contracts.DataModels.MasterVenueLayoutSectionSeat> masterVenueLayoutSectionSeat = AutoMapper.Mapper.Map<List<Contracts.DataModels.MasterVenueLayoutSectionSeat>>(_masterVenueLayoutSectionSeatRepository.GetByMasterVenueLayoutSectionId(masterVenueLayoutSection.Id));

                for (int k = 0; k < masterVenueLayoutSectionSeat.Count; k = k + 2000)
                {
                    var matchSeatTicketDetailListBatcher = masterVenueLayoutSectionSeat.Skip(k).Take(2000);

                    var MatchLayoutSectionSeatIds = matchSeatTicketDetailListBatcher.Select(s => s.MasterVenueLayoutSectionId).FirstOrDefault();

                    List<Contracts.DataModels.MasterVenueLayoutSectionSeat> matchLayoutSectionSeats = AutoMapper.Mapper.Map<List<Contracts.DataModels.MasterVenueLayoutSectionSeat>>(_masterVenueLayoutSectionSeatRepository.GetByMasterVenueLayoutSectionId(MatchLayoutSectionSeatIds));

                    List<Contracts.Models.MasterVenueLayoutSectionSeat> MatchLayoutSectionSeats = AutoMapper.Mapper.Map<List<Contracts.Models.MasterVenueLayoutSectionSeat>>(matchLayoutSectionSeats);

                    foreach (var item in MatchLayoutSectionSeats)
                    {
                        MasterVenueLayoutSectionSeatList.Add(item);
                    }
                }

                var orderByRowColumnOrder = MasterVenueLayoutSectionSeatList.OrderByDescending(g => g.RowOrder).ThenBy(g => g.ColumnOrder).ToList();
                List<FIL.Contracts.Models.MasterVenueSeatItem> seatItems = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.MasterVenueSeatItem>>(orderByRowColumnOrder);
                List<FIL.Contracts.Models.MasterVenueRow> Rows = new List<FIL.Contracts.Models.MasterVenueRow>();
                var groupByRowNumber = seatItems.GroupBy(g => g.RowNumber);

                foreach (var item in groupByRowNumber)
                {
                    var seat = seatItems.Where(s => s.RowNumber == item.Key.ToString()).ToList();
                    FIL.Contracts.Models.MasterVenueRow Row = new FIL.Contracts.Models.MasterVenueRow();
                    Row.MasterVenueSeatItems = seat;
                    Rows.Add(Row);
                }
                return new MasterVenueViewSeatLayoutQueryResult
                {
                    MasterVenueRows = Rows,
                    IsSeatLayout = isSeatLayout
                };
            }
            catch (Exception ex)
            {
            }

            return new MasterVenueViewSeatLayoutQueryResult
            {
                MasterVenueRows = null,
            };
        }
    }
}