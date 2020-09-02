using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class ApprovePartialVoidRequestDataQueryHandler : IQueryHandler<ApprovePartialVoidRequestDataQuery, ApprovePartialVoidRequestDataQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly IPartialVoidRequestDetailRepository _partialVoidRequestDetailRepository;

        public ApprovePartialVoidRequestDataQueryHandler(IUserRepository userRepository, IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository, IPartialVoidRequestDetailRepository partialVoidRequestDetailRepository)
        {
            _userRepository = userRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
            _partialVoidRequestDetailRepository = partialVoidRequestDetailRepository;
        }

        public ApprovePartialVoidRequestDataQueryResult Handle(ApprovePartialVoidRequestDataQuery query)
        {
            var user = _userRepository.GetByAltId(query.AltId);
            var boxofficeuser = _boxofficeUserAdditionalDetailRepository.GetAllById(user.Id);
            var userids = _userRepository.GetByUserIds(boxofficeuser.Select(s => s.UserId)).ToList();
            var partialvoidrequestDetails = _partialVoidRequestDetailRepository.GetAllByUserAltId(userids.Select(s => s.AltId)).ToList();
            var usernames = _userRepository.GetByAltIds(partialvoidrequestDetails.Select(s => s.CreatedBy)).ToList();

            return new ApprovePartialVoidRequestDataQueryResult
            {
                PartialVoidRequestDetail = AutoMapper.Mapper.Map<List<PartialVoidRequestDetail>>(partialvoidrequestDetails),
                Users = AutoMapper.Mapper.Map<List<User>>(usernames)
            };
        }
    }
}