using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Logging;

namespace FIL.Api.Integrations
{
    public interface IService
    {
    }

    public abstract class Service<T> : IService
    {
        protected readonly ILogger _logger;
        protected readonly ISettings _settings;

        protected Service(ILogger logger, ISettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        protected IResponse<T> GetResponse(bool success, T result)
        {
            return new Response<T>
            {
                Success = success,
                Result = result
            };
        }
    }
}