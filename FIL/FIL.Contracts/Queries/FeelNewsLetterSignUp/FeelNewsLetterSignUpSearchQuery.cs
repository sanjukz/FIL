using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelNewsLetterSignUp;

namespace FIL.Contracts.Queries.FeelNewsLetterSignUp
{
    public class FeelNewsLetterSignUpSearchQuery : IQuery<FeelNewsLetterSignUpSearchQueryResult>
    {
        public string Email { get; set; }
        public bool IsFeel { get; set; }
    }
}