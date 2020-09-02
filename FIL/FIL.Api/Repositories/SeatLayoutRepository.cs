using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Data;

namespace FIL.Api.Repositories
{
    public interface ISeatLayoutRepository : IOrmRepository<SeatLayout, SeatLayout>
    {
        SeatLayoutData GetSeasonSeatLayoutData(long eventTicketattributeId);
    }

    public class SeatLayoutRepository : BaseOrmRepository<SeatLayout>, ISeatLayoutRepository
    {
        public SeatLayoutRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public SeatLayoutData GetSeasonSeatLayoutData(long eventTicketattributeId)
        {
            SeatLayoutData seatLayoutData = new SeatLayoutData();
            var SeasonseatLayout = GetCurrentConnection().QueryMultiple("spGetBoxofficeSeasonSeatLayout", new { EventTicketAttributeId = eventTicketattributeId }, commandType: CommandType.StoredProcedure);
            seatLayoutData.SeatLayouts = SeasonseatLayout.Read<SeatLayout>();
            return seatLayoutData;
        }
    }
}