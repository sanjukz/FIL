using FIL.Api.Repositories;
using FIL.Contracts.Queries.Zipcode;
using FIL.Contracts.QueryResults;

namespace FIL.Api.QueryHandlers.Zipcodes
{
    public class ZipcodeSearchQueryHandler : IQueryHandler<ZipcodeSearchQuery, ZipcodeSearchQueryResult>
    {
        private readonly IZipcodeRepository _zipRepository;

        public ZipcodeSearchQueryHandler(IZipcodeRepository zipRepository)
        {
            _zipRepository = zipRepository;
        }

        public ZipcodeSearchQueryResult Handle(ZipcodeSearchQuery query)
        {
            var zipcodeResult = _zipRepository.GetByZipcode(query.Zipcode);
            return new ZipcodeSearchQueryResult
            {
                IsExisting = zipcodeResult != null,
            };
        }
    }
}