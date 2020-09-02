using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelRedemption;

namespace FIL.Contracts.Queries.FeelRedemption
{
    public class UserLoginUserQuery : IQuery<UserLoginUserQueryResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Channels? ChannelId { get; set; }
        public SignUpMethods? SignUpMethodId { get; set; }
    }
}