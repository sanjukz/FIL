using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class ReprintRequestDataQueryHandler : IQueryHandler<ReprintRequestDataQuery, ReprintRequestDataQueryResult>
    {
        private readonly IReprintRequestRepository _reprintRequestRepository;
        private readonly IReprintRequestDetailRepository _reprintRequestDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;

        public ReprintRequestDataQueryHandler(IReprintRequestRepository reprintRequestRepository, IReprintRequestDetailRepository reprintRequestDetailRepository, IUserRepository userRepository, IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository)
        {
            _reprintRequestRepository = reprintRequestRepository;
            _reprintRequestDetailRepository = reprintRequestDetailRepository;
            _userRepository = userRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
        }

        public ReprintRequestDataQueryResult Handle(ReprintRequestDataQuery query)
        {
            var user = _userRepository.GetByAltId(query.AltId);
            var reprintRequest = _reprintRequestRepository.GetByUserAltId(query.AltId);
            var reprintRequestDetails = _reprintRequestDetailRepository.GetByReprintRequestId(reprintRequest.Select(s => s.Id));
            return new ReprintRequestDataQueryResult
            {
                User = AutoMapper.Mapper.Map<User>(user),
                ReprintRequestDetail = AutoMapper.Mapper.Map<IEnumerable<ReprintRequestDetail>>(reprintRequestDetails.OrderByDescending(o => o.RePrintRequestId)),
                ReprintRequest = AutoMapper.Mapper.Map<IEnumerable<ReprintRequest>>(reprintRequest),
            };
        }
    }
}