using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Models.TMS;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IStandDetailRepository : IOrmRepository<StandDetail, StandDetail>
    {
        List<StandDetail> GetStandData(long eventDetailId, long ticketCategoryId);

        List<StandDetail> GetStandDataByVenue(long eventId, int venueId, long ticketCategoryId);
    }

    public class StandDetailRepository : BaseOrmRepository<StandDetail>, IStandDetailRepository
    {
        public StandDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public List<StandDetail> GetStandData(long eventDetailId, long ticketCategoryId)
        {
            var SeasonseatLayoutlist = GetCurrentConnection().QueryMultiple("spGetStandDetails", new { EventDetailId = eventDetailId, TicketCategoryId = ticketCategoryId }, commandType: CommandType.StoredProcedure);
            return SeasonseatLayoutlist.Read<StandDetail>().ToList();
        }

        public List<StandDetail> GetStandDataByVenue(long eventId, int venueId, long ticketCategoryId)
        {
            var SeasonseatLayoutlist = GetCurrentConnection().QueryMultiple("spGetStandDetailsByVenue", new { EventId = eventId, VenueId = venueId, TicketCategoryId = ticketCategoryId }, commandType: CommandType.StoredProcedure);
            return SeasonseatLayoutlist.Read<StandDetail>().ToList();
        }
    }
}