using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetPahInfoQueryHandler : IQueryHandler<GetPahInfoQuery, GetPahInfoQueryResult>
    {
        private readonly IMatchSeatPahDataRepository _matchSeatPahDataRepository;
        private readonly FIL.Logging.ILogger _logger;

        public GetPahInfoQueryHandler(IMatchSeatPahDataRepository matchSeatPahDataRepository, FIL.Logging.ILogger logger)
        {
            _matchSeatPahDataRepository = matchSeatPahDataRepository;
            _logger = logger;
        }

        public GetPahInfoQueryResult Handle(GetPahInfoQuery query)
        {
            try
            {
                MatchSeatTicketData matchSeatTicketData = AutoMapper.Mapper.Map<MatchSeatTicketData>(_matchSeatPahDataRepository.GetMatchSeatTicketInfo(query.TransactionId, query.ModifiedBy));
                return new GetPahInfoQueryResult
                {
                    matchSeatTicketInfo = matchSeatTicketData.matchSeatTicketInfo,
                    matchTeamInfo = matchSeatTicketData.matchTeamInfo
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
            return new GetPahInfoQueryResult
            {
                matchSeatTicketInfo = null,
                matchTeamInfo = null
            };
        }
    }
}