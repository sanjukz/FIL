using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.User;

namespace FIL.Contracts.Queries.User
{
    public class UserSearchQuery : IQuery<UserSearchQueryResult>
    {
        public string Email { get; set; }
        public Channels? ChannelId { get; set; }
        public SignUpMethods? SignUpMethodId { get; set; }
        public Site? SiteId { get; set; }
        public string SocialLoginId { get; set; }
        public string PhoneCode { get; set; }
    }
}