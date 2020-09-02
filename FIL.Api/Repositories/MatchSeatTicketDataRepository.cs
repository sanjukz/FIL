using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Data;

namespace FIL.Api.Repositories
{
    public interface IMatchSeatTicketDataRepository : IOrmRepository<MatchSeatTicketData, MatchSeatTicketData>
    {
        MatchSeatTicketData GetMatchSeatTicketInfo(long transactionId, Guid transactionAltId, string matchSeatTicketDetailIds, Guid modifiedBy, bool isRePrint);
    }

    public class MatchSeatTicketDataRepository : BaseOrmRepository<MatchSeatTicketData>, IMatchSeatTicketDataRepository
    {
        public MatchSeatTicketDataRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MatchSeatTicketData GetMatchSeatTicketInfo(long transactionId, Guid transactionAltId, string matchSeatTicketDetailIds, Guid modifiedBy, bool isRePrint)
        {
            MatchSeatTicketData matchSeatTicketData = new MatchSeatTicketData();
            if (isRePrint)
            {
                var matchSeatTicketDetails = GetCurrentConnection().QueryMultiple("spGetReprintTicketDetails", new { MatchSeatTicketDetailIds = matchSeatTicketDetailIds, ModifiedBy = modifiedBy }, commandType: CommandType.StoredProcedure);
                matchSeatTicketData.matchSeatTicketInfo = matchSeatTicketDetails.Read<MatchSeatTicketInfo>();
                matchSeatTicketData.matchTeamInfo = matchSeatTicketDetails.Read<MatchTeamInfo>();
                return matchSeatTicketData;
            }
            else
            {
                var matchSeatTicketDetails = GetCurrentConnection().QueryMultiple("spGetTicketDetails", new { TransactionId = transactionId, TransactionAltId = transactionAltId, ModifiedBy = modifiedBy }, commandType: CommandType.StoredProcedure);
                matchSeatTicketData.matchSeatTicketInfo = matchSeatTicketDetails.Read<MatchSeatTicketInfo>();
                matchSeatTicketData.matchTeamInfo = matchSeatTicketDetails.Read<MatchTeamInfo>();
                return matchSeatTicketData;
            }
        }
    }
}