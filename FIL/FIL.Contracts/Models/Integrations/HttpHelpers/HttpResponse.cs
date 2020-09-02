using System.Net;

namespace FIL.Contracts.Models.Integrations.HttpHelpers
{
    public interface IHttpResponse : IError
    {
        string Response { get; set; }
        WebException WebException { get; set; }
    }

    public class HttpResponse : Error, IHttpResponse
    {
        public string Response { get; set; }
        public WebException WebException { get; set; }
    }
}