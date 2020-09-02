using FIL.Api.Repositories;
using FIL.Contracts.Queries.Account;
using FIL.Contracts.QueryResults.Account;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Account
{
    public class GetAllGuestUserDetailQueryHandler : IQueryHandler<GetAllGuestUserDetailQuery, GetAllGuestUserDetailQueryResult>
    {
        private readonly IGuestUserAdditionalDetailRepository _guestUserAdditionalDetailRepository;

        public GetAllGuestUserDetailQueryHandler(IGuestUserAdditionalDetailRepository guestUserAdditionalDetailRepository)
        {
            _guestUserAdditionalDetailRepository = guestUserAdditionalDetailRepository;
        }

        public GetAllGuestUserDetailQueryResult Handle(GetAllGuestUserDetailQuery query)
        {
            var guestUserDetails = _guestUserAdditionalDetailRepository.GetByUserId(query.UserId).ToList();

            return new GetAllGuestUserDetailQueryResult
            {
                GuestUserDetails = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.GuestUserAdditionalDetail>>(guestUserDetails)
            };
        }
    }
}