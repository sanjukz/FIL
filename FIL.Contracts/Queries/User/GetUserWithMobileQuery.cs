using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.User;

namespace FIL.Contracts.Queries.User
{
    public class GetUserWithMobileQuery : IQuery<GetUserWithMobileQueryResult>
    {
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public Channels? ChannelId { get; set; }
    }
}