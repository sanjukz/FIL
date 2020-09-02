using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.NewsLetterSignUp;

namespace FIL.Contracts.Queries.NewsLetterSignUp
{
    public class NewsLetterSignUpSearchQuery : IQuery<NewsLetterSignUpSearchQueryResult>
    {
        public string Email { get; set; }
    }
}