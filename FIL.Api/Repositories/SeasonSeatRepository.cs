using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Data;

namespace FIL.Api.Repositories
{
    public interface ISeasonSeatRepository : IOrmRepository<SeasonSeat, SeasonSeat>
    {
        SeasonSeatInfo GetSeasonSeatInfo(long eventTicketattributeId, string SeatTag);
    }

    public class SeasonSeatRepository : BaseOrmRepository<SeasonSeat>, ISeasonSeatRepository
    {
        public SeasonSeatRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public SeasonSeatInfo GetSeasonSeatInfo(long eventTicketattributeId, string seatTag)
        {
            SeasonSeatInfo seasonSeatInfo = new SeasonSeatInfo();
            var seasonSeat = GetCurrentConnection().QueryMultiple("spGetSeasonSeatDetails", new { EventTicketAttributeId = eventTicketattributeId, SeatTag = seatTag }, commandType: CommandType.StoredProcedure);
            seasonSeatInfo.SeasonSeats = seasonSeat.Read<SeasonSeat>();
            return seasonSeatInfo;
        }
    }
}