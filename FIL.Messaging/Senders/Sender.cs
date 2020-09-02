using FIL.Configuration;
using FIL.Logging;
using FIL.Messaging.Models;
using System.Threading.Tasks;

namespace FIL.Messaging.Senders
{
    public interface ISender<in T>
    {
        bool IsSendingDisabled { get; }

        /// <summary>
        /// Allows disabiling of messaging for testing, etc.
        /// </summary>
        void DisableSending();

        Task<IResponse> Send(T message);
    }

    public abstract class Sender<T> : ISender<T>
    {
        protected readonly ILogger _logger;
        protected readonly ISettings _settings;

        public bool IsSendingDisabled { get; protected set; }

        internal Sender(ILogger logger, ISettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public abstract Task<IResponse> Send(T message);

        public void DisableSending()
        {
            IsSendingDisabled = true;
        }

        protected IResponse GetResponse(bool success = true, string errorMessage = null)
        {
            return new Response
            {
                Success = success,
                ErrorMessage = errorMessage
            };
        }
    }
}