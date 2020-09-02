using FIL.Api.Repositories;
using FIL.Contracts.Queries;
using FIL.Contracts.QueryResults;
using System;

namespace FIL.Api.QueryHandlers
{
    public class FeelItLiveHostDetailQueryHandler : IQueryHandler<FeelItLiveHostQuery, FeelItLiveHostQueryResult>
    {
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly Logging.ILogger _logger;

        public FeelItLiveHostDetailQueryHandler(Logging.ILogger logger, IEventHostMappingRepository eventHostMappingRepository)
        {
            _eventHostMappingRepository = eventHostMappingRepository;
            _logger = logger;
        }

        public FeelItLiveHostQueryResult Handle(FeelItLiveHostQuery query)
        {
            try
            {
                var result = _eventHostMappingRepository.GetLatestByEmail(query.Email);
                return new FeelItLiveHostQueryResult
                {
                    AltId = (System.Guid)result.AltId,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Description = result.Description,
                    Email = result.Email
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new FeelItLiveHostQueryResult();
                throw ex;
            }
        }
    }
}