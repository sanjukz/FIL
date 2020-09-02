using FIL.Api.Repositories;
using FIL.Contracts.Queries.FeelNewsLetterSignUp;
using FIL.Contracts.QueryResults.FeelNewsLetterSignUp;

namespace FIL.Api.QueryHandlers.FeelNewsLetterSignUpSearch
{
    public class FeelNewsLetterSignUpSearchQueryHandler : IQueryHandler<FeelNewsLetterSignUpSearchQuery, FeelNewsLetterSignUpSearchQueryResult>
    {
        private readonly INewsLetterSignUpRepository _newsLetterSignUpRepository;

        public FeelNewsLetterSignUpSearchQueryHandler(INewsLetterSignUpRepository newsLetterSignUpRepository)
        {
            _newsLetterSignUpRepository = newsLetterSignUpRepository;
        }

        public FeelNewsLetterSignUpSearchQueryResult Handle(FeelNewsLetterSignUpSearchQuery query)
        {
            var response = new FeelNewsLetterSignUpSearchQueryResult();
            var user = _newsLetterSignUpRepository.GetByEmailFeel(query.Email, query.IsFeel);
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