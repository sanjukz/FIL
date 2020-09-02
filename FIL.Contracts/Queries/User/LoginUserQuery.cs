using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;

namespace FIL.Contracts.Queries.User
{
    public class LoginUserQuery : IQuery<LoginUserQueryResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsCalledFromCheckout { get; set; }
        public Channels? ChannelId { get; set; }
        public SignUpMethods? SignUpMethodId { get; set; }
        public Site? SiteId { get; set; }
    }
}