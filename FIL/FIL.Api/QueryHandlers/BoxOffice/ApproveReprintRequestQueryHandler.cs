using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class ApproveReprintRequestQueryHandler : IQueryHandler<ApproveReprintRequestQuery, ApproveReprintRequestQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly IReprintRequestRepository _reprintRequestRepository;
        private readonly IReprintRequestDetailRepository _reprintRequestDetailRepository;

        public ApproveReprintRequestQueryHandler(IUserRepository userRepository, IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository, IReprintRequestRepository reprintRequestRepository, IReprintRequestDetailRepository reprintRequestDetailRepository)
        {
            _userRepository = userRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
            _reprintRequestRepository = reprintRequestRepository;
            _reprintRequestDetailRepository = reprintRequestDetailRepository;
        }

        public ApproveReprintRequestQueryResult Handle(ApproveReprintRequestQuery query)
        {
            var user = _userRepository.GetByAltId(query.AltId);
            var boxofficeuser = _boxofficeUserAdditionalDetailRepository.GetAllById(user.Id);
            var userids = _userRepository.GetByUserIds(boxofficeuser.Select(s => s.UserId)).ToList();
            var reprintRequests = _reprintRequestRepository.GetAllByUserAltId(userids.Select(s => s.AltId)).ToList();
            var usernames = _userRepository.GetByUserIds(reprintRequests.Select(s => s.UserId)).ToList();
            var reprintRequestdetails = _reprintRequestDetailRepository.GetByReprintRequestIdForApprove(reprintRequests.Select(s => s.Id)).ToList();
            return new ApproveReprintRequestQueryResult
            {
                ReprintRequests = AutoMapper.Mapper.Map<List<ReprintRequest>>(reprintRequests),
                ReprintRequestDetail = AutoMapper.Mapper.Map<List<ReprintRequestDetail>>(reprintRequestdetails),
                Users = AutoMapper.Mapper.Map<List<User>>(usernames)
            };
        }
    }
}