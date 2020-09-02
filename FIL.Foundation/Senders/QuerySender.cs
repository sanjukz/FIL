using FIL.Contracts.Interfaces.Queries;
using FIL.Http;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace FIL.Foundation.Senders
{
    public interface IQuerySender
    {
        Task<TR> Send<TR>(IQuery<TR> query);
    }

    public class QuerySender : IQuerySender
    {
        private readonly IRestHelper _restHelper;
        private readonly MediaTypeFormatter _formatter;

        public QuerySender(IRestHelper restHelper)
        {
            _restHelper = restHelper;
            _formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }
            };
        }

        public Task<TR> Send<TR>(IQuery<TR> query)
        {
            return _restHelper.Post<TR>($"api/query/{query.GetType().Name}", query, _formatter);
        }
    }
}