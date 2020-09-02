using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.QueryResults.Redemption;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.Redemption
{
    public class LanguagesQueryHandler : IQueryHandler<LanguagesQuery, LanguagesQueryResult>
    {
        private readonly ILanguagesRepository _LanguagesRepository;
        private readonly FIL.Logging.ILogger _logger;

        public LanguagesQueryHandler(ILanguagesRepository LanguagesRepository, FIL.Logging.ILogger logger)
        {
            _LanguagesRepository = LanguagesRepository;
            _logger = logger;
        }

        public LanguagesQueryResult Handle(LanguagesQuery query)
        {
            try
            {
                var Languages = _LanguagesRepository.GetAll().ToList();
                return new LanguagesQueryResult
                {
                    Languages = Languages
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
            return new LanguagesQueryResult
            {
            };
        }
    }
}