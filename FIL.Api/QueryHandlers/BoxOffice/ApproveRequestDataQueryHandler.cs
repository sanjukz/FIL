using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class ApproveRequestDataQueryHandler : IQueryHandler<ApproveRequestDataQuery, ApproveRequestDataQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly IVoidRequestDetailRepository _voidRequestDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;

        public ApproveRequestDataQueryHandler(IUserRepository userRepository, IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository, IVoidRequestDetailRepository voidRequestDetailRepository, ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository)
        {
            _userRepository = userRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
            _voidRequestDetailRepository = voidRequestDetailRepository;
            _transactionRepository = transactionRepository;
        }

        public ApproveRequestDataQueryResult Handle(ApproveRequestDataQuery query)
        {
            var user = _userRepository.GetByAltId(query.AltId);
            var boxofficeuser = _boxofficeUserAdditionalDetailRepository.GetAllById(user.Id);
            var userids = _userRepository.GetByUserIds(boxofficeuser.Select(s => s.UserId));
            var voidrequestdetail = _voidRequestDetailRepository.GetByAltIds(userids.Select(s => s.AltId)).OrderBy(s => s.TransactionId);
            var transaction = _transactionRepository.GetByAllTransactionIds(voidrequestdetail.Select(s => s.TransactionId)).OrderBy(s => s.Id);
            var approveVoidRequestContainer = voidrequestdetail.Select(cb =>
            {
                var cUsers = _userRepository.GetByAltId(cb.CreatedBy);
                return new ApproveVoidRequestContainer
                {
                    User = AutoMapper.Mapper.Map<User>(cUsers),
                };
            });
            var users = _userRepository.GetByAltIds(voidrequestdetail.Select(s => s.CreatedBy));
            return new ApproveRequestDataQueryResult
            {
                VoidRequestDetail = AutoMapper.Mapper.Map<IEnumerable<VoidRequestDetail>>(voidrequestdetail),
                Transaction = AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.Transaction>>(transaction),
                ApproveVoidRequestContainer = approveVoidRequestContainer.ToList(),
            };
        }
    }
}