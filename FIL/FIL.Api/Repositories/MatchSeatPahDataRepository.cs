using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Data;

namespace FIL.Api.Repositories
{
    public interface IMatchSeatPahDataRepository : IOrmRepository<MatchSeatTicketData, MatchSeatTicketData>
    {
        MatchSeatTicketData GetMatchSeatTicketInfo(long transactionId, Guid modifiedBy);
    }

    public class MatchSeatPahDataRepository : BaseOrmRepository<MatchSeatTicketData>, IMatchSeatPahDataRepository
    {
        public MatchSeatPahDataRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MatchSeatTicketData GetMatchSeatTicketInfo(long transactionId, Guid modifiedBy)
        {
            MatchSeatTicketData matchSeatTicketData = new MatchSeatTicketData();
            var matchSeatTicketDetails = GetCurrentConnection().QueryMultiple("spGetTicketDetailsPah", new { TransactionId = transactionId, ModifiedBy = modifiedBy }, commandType: CommandType.StoredProcedure);
            matchSeatTicketData.matchSeatTicketInfo = matchSeatTicketDetails.Read<MatchSeatTicketInfo>();
            matchSeatTicketData.matchTeamInfo = matchSeatTicketDetails.Read<MatchTeamInfo>();
            return matchSeatTicketData;
        }
    }
}