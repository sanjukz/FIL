using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.QueryResults.Redemption;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.Redemption
{
    public class ServicesQueryHandler : IQueryHandler<ServicesQuery, ServicesQueryResult>
    {
        private readonly IServicesRepository _ServicesRepository;
        private readonly FIL.Logging.ILogger _logger;

        public ServicesQueryHandler(IServicesRepository ServicesRepository, FIL.Logging.ILogger logger)
        {
            _ServicesRepository = ServicesRepository;
            _logger = logger;
        }

        public ServicesQueryResult Handle(ServicesQuery query)
        {
            try
            {
                var services = _ServicesRepository.GetAll().ToList();
                return new ServicesQueryResult
                {
                    Services = services
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }
            return new ServicesQueryResult
            {
            };
        }
    }
}