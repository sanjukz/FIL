using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.BoxOffice;

namespace FIL.Contracts.Queries.BoxOffice
{
    public class GetParentUserQuery : IQuery<GetParentUserQueryResult>
    {
        public int RoleId { get; set; }
    }
}