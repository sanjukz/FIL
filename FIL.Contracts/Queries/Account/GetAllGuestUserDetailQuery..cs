using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Account;

namespace FIL.Contracts.Queries.Account
{
    public class GetAllGuestUserDetailQuery : IQuery<GetAllGuestUserDetailQueryResult>
    {
        public long UserId { get; set; }
    }
}