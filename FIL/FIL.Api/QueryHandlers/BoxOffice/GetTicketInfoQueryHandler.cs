using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetTicketInfoQueryHandler : IQueryHandler<GetTicketInfoQuery, GetTicketInfoQueryResult>
    {
        private readonly IMatchSeatTicketDataRepository _matchSeatTicketDataRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public GetTicketInfoQueryHandler(IMatchSeatTicketDataRepository matchSeatTicketDataRepository, IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, FIL.Logging.ILogger logger)
        {
            _matchSeatTicketDataRepository = matchSeatTicketDataRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _logger = logger;
        }

        public GetTicketInfoQueryResult Handle(GetTicketInfoQuery query)
        {
            try
            {
                string matchSeatTicketDetailIds = string.Empty;
                var matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByBarcodeList(query.BarcodeNumbers);

                foreach (var item in matchSeatTicketDetail)
                {
                    matchSeatTicketDetailIds += item.Id.ToString() + ",";
                }
                MatchSeatTicketData matchSeatTicketData = AutoMapper.Mapper.Map<MatchSeatTicketData>(_matchSeatTicketDataRepository.GetMatchSeatTicketInfo(query.TransactionId, query.TransactionAltId, matchSeatTicketDetailIds, query.ModifiedBy, query.IsRePrint));
                return new GetTicketInfoQueryResult
                {
                    matchSeatTicketInfo = matchSeatTicketData.matchSeatTicketInfo,
                    matchTeamInfo = matchSeatTicketData.matchTeamInfo
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
            return new GetTicketInfoQueryResult
            {
                matchSeatTicketInfo = null,
                matchTeamInfo = null
            };
        }
    }
}