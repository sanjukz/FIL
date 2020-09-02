using FIL.Contracts.Models.Integrations.HttpHelpers;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.HttpHelpers
{
    public class HttpWebRequestProviders<T>
    {
        public static async Task<IHttpResponse> GetWebRequestProviderAsync(string url, IRequestCreateOptions<T> requestCreateOptions)
        {
            var request = HttpWebRequestHelpers<T>.GetWebRequest(url, "GET", requestCreateOptions).Result;
            return await HttpWebRequestHelpers<T>.ExecuteWebRequest(request).ConfigureAwait(false);
        }

        public static async Task<IHttpResponse> PostWebRequestProviderAsync(string url, IRequestCreateOptions<T> requestCreateOptions)
        {
            var request = HttpWebRequestHelpers<T>.GetWebRequest(url, "POST", requestCreateOptions).Result;
            return await HttpWebRequestHelpers<T>.ExecuteWebRequest(request).ConfigureAwait(false);
        }

        public static async Task<IHttpResponse> PutWebRequestProvider(string url, IRequestCreateOptions<T> requestCreateOptions)
        {
            var request = HttpWebRequestHelpers<T>.GetWebRequest(url, "PUT", requestCreateOptions).Result;
            return await HttpWebRequestHelpers<T>.ExecuteWebRequest(request).ConfigureAwait(false);
        }

        public static async Task<IHttpResponse> DeleteWebRequestProvider(string url, IRequestCreateOptions<T> requestCreateOptions)
        {
            var request = HttpWebRequestHelpers<T>.GetWebRequest(url, "DELETE", requestCreateOptions).Result;
            return await HttpWebRequestHelpers<T>.ExecuteWebRequest(request).ConfigureAwait(false);
        }

        public static async Task<IHttpResponse> GetBearerWebRequestProvider(string url, IRequestCreateOptions<T> requestCreateOptions)
        {
            var request = HttpWebRequestHelpers<T>.GetWebRequest(url, "GET", requestCreateOptions, true).Result;
            return await HttpWebRequestHelpers<T>.ExecuteWebRequest(request).ConfigureAwait(false);
        }

        public static async Task<IHttpResponse> PostBearerWebRequestProvider(string url, IRequestCreateOptions<T> requestCreateOptions)
        {
            var request = HttpWebRequestHelpers<T>.GetWebRequest(url, "POST", requestCreateOptions, true).Result;
            return await HttpWebRequestHelpers<T>.ExecuteWebRequest(request).ConfigureAwait(false);
        }

        public static async Task<IHttpResponse> PutBearerWebRequestProvider(string url, IRequestCreateOptions<T> requestCreateOptions)
        {
            var request = HttpWebRequestHelpers<T>.GetWebRequest(url, "PUT", requestCreateOptions, true).Result;
            return await HttpWebRequestHelpers<T>.ExecuteWebRequest(request).ConfigureAwait(false);
        }
    }
}