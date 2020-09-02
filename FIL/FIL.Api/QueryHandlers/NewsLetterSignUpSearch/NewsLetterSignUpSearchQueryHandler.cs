using FIL.Api.Repositories;
using FIL.Contracts.Queries.NewsLetterSignUp;
using FIL.Contracts.QueryResults.NewsLetterSignUp;

namespace FIL.Api.QueryHandlers.NewsLetterSignUpSearch
{
    public class NewsLetterSignUpSearchQueryHandler : IQueryHandler<NewsLetterSignUpSearchQuery, NewsLetterSignUpSearchQueryResult>
    {
        private readonly INewsLetterSignUpRepository _newsLetterSignUpRepository;

        public NewsLetterSignUpSearchQueryHandler(INewsLetterSignUpRepository newsLetterSignUpRepository)
        {
            _newsLetterSignUpRepository = newsLetterSignUpRepository;
        }

        public NewsLetterSignUpSearchQueryResult Handle(NewsLetterSignUpSearchQuery query)
        {
            var response = new NewsLetterSignUpSearchQueryResult();
            var user = _newsLetterSignUpRepository.GetByEmail(query.Email);
            if (user != null)
            {
                response.IsExisting = true;
            }
            else
            {
                response.IsExisting = false;
            }
            return response;
        }
    }
}