using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace FIL.Http
{
    public interface IRestHelper : IDisposable
    {
        Uri BaseAddress { get; }
        TimeSpan ClientTimeout { get; }

        /// <summary>
        /// Gets a dictionary of all results.
        /// </summary>
        Task<IDictionary<string, T>> GetDictionaryResults<T>(string endPoint);

        Task<T> Post<T>(string endPoint, object model, MediaTypeFormatter formatter);

        Task Post(string endPoint, object model, MediaTypeFormatter formatter);

        /// <summary>
        /// Gets all results.
        /// </summary>
        Task<IEnumerable<T>> GetAllResults<T>(string endPoint);

        Task<T> GetResultByHeader<T>(string endPoint, Dictionary<string, string> headers, bool? isTimeOut = false);
        /// <summary>
        /// Gets the scalar result.
        /// </summary>
        Task<T> GetScalarResult<T>(string endPoint);

        /// <summary>
        /// Gets the result.
        /// </summary>
        Task<T> GetResult<T>(string endPoint);

        /// <summary>
        /// Posts the containerless result.
        /// </summary>
        Task<T> PostResult<T>(object model, string endPoint);

        Task<T> PostScalarResult<T>(object model, string endPoint);

        /// <summary>
        /// Posts result that doesn't return anything.
        /// </summary>
        Task PostVoidResult(object model, string endPoint);

        /// <summary>
        /// Posts a form.
        /// </summary>
        Task PostForm(FormUrlEncodedContent content, string endPoint);

        /// <summary>
        /// Posts result that returns guid.
        /// </summary>
        Task<Guid> PostGuidResult(object model, string endPoint);

        /// <summary>
        /// Upload a file stream to an endpoint.
        /// </summary>
        Task<T> PostStream<T>(Stream stream, string fileName, string endPoint);

        /// <summary>
        /// Gets the response from an endpoint as a stream.
        /// </summary>
        Task<Stream> GetStream(string endPoint);

        /// <summary>
        /// Makes an asynchronous delete call.
        /// </summary>
        Task Delete(string endPoint);
    }

    public class RestHelper : IRestHelper
    {
        private readonly HttpClient _httpClient;
        public TimeSpan ClientTimeout => GetHttpClient().Timeout;

        public Uri BaseAddress { get; }

        public RestHelper(Uri hostName)
            : this(hostName, TimeSpan.FromSeconds(120))
        {
        }

        public RestHelper(Uri hostName, TimeSpan timeout)
        {
            _httpClient = new HttpClient
            {
                Timeout = timeout,
                BaseAddress = BaseAddress = hostName
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IDictionary<string, T>> GetDictionaryResults<T>(string endPoint)
        {
            IDictionary<string, T> result = null;
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPoint).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync().ContinueWith(j => JsonConvert.DeserializeObject<IDictionary<string, T>>(j.Result)).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
            return result;
        }

        public async Task<IEnumerable<T>> GetAllResults<T>(string endPoint)
        {
            IEnumerable<T> models = null;
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPoint).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                models = await response.Content.ReadAsStringAsync().ContinueWith(j => JsonConvert.DeserializeObject<IEnumerable<T>>(j.Result)).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
            return models;
        }

        public async Task<T> GetScalarResult<T>(string endPoint)
        {
            T result = default(T);
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPoint).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync().ContinueWith(j => JsonConvert.DeserializeObject<T>(j.Result)).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
            return result;
        }

        public async Task<T> GetResult<T>(string endPoint)
        {
            T model = default(T);
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPoint).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsStringAsync().ContinueWith(j => JsonConvert.DeserializeObject<T>(j.Result)).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
            return model;
        }

        public async Task<T> GetResultByHeader<T>(string endPoint, Dictionary<string, string> headers, bool? isTimeOut = false)
        {
            T model = default(T);
            var httpClient = GetHttpClient();
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Remove(header.Key);
            }
            var baseAddress = new Uri(endPoint);
            if ((bool)isTimeOut)
            {
                httpClient.Timeout = new TimeSpan(1, 0, 0);
            }
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
            HttpResponseMessage response = await httpClient.GetAsync(baseAddress).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                model = await response.Content.ReadAsStringAsync().ContinueWith(j => JsonConvert.DeserializeObject<T>(j.Result)).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
            return model;
        }

        public async Task<T> PostResult<T>(object model, string endPoint)
        {
            T modelResult = default(T);
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await GetHttpClient().PostAsync(endPoint, stringContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                modelResult = await response.Content.ReadAsStringAsync().ContinueWith(j => JsonConvert.DeserializeObject<T>(j.Result)).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
            return modelResult;
        }

        public async Task<T> PostScalarResult<T>(object model, string endPoint)
        {
            T modelResult = default(T);
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await GetHttpClient().PostAsync(endPoint, stringContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                modelResult = await response.Content.ReadAsStringAsync().ContinueWith(j =>
                {
                    return JsonConvert.DeserializeObject<T>(j.Result);
                }).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
            return modelResult;
        }

        public async Task<T> Post<T>(string endPoint, object model, MediaTypeFormatter formatter)
        {
            T modelResult = default(T);
            HttpResponseMessage response = await GetHttpClient().PostAsync(endPoint, model, formatter).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                modelResult = await response.Content.ReadAsAsync<T>(new List<MediaTypeFormatter> { formatter }).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
            return modelResult;
        }

        public async Task Post(string endpoint, object model, MediaTypeFormatter formatter)
        {
            HttpResponseMessage response = await GetHttpClient().PostAsync(endpoint, model, formatter).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpResponseMessage(response, endpoint).ConfigureAwait(false);
            }
        }

        public async Task PostVoidResult(object model, string endPoint)
        {
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await GetHttpClient().PostAsync(endPoint, stringContent).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
        }

        public async Task PostForm(FormUrlEncodedContent content, string endPoint)
        {
            HttpResponseMessage response = await GetHttpClient().PostAsync(endPoint, content).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
        }

        public async Task<Guid> PostGuidResult(object model, string endPoint)
        {
            Guid modelResult = Guid.Empty;
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await GetHttpClient().PostAsync(endPoint, stringContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                modelResult = await response.Content.ReadAsStringAsync().ContinueWith(j =>
                {
                    return JsonConvert.DeserializeObject<Guid>(j.Result);
                }).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
            return modelResult;
        }

        public async Task<T> PostStream<T>(Stream stream, string fileName, string endPoint)
        {
            T modelResult = default(T);

            // Create the form.
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            content.Add(fileContent);

            HttpResponseMessage response = await GetHttpClient().PostAsync(endPoint, content).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                modelResult = await response.Content.ReadAsStringAsync().ContinueWith(j => JsonConvert.DeserializeObject<T>(j.Result)).ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }

            return modelResult;
        }

        public async Task<Stream> GetStream(string endPoint)
        {
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPoint).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
            else
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
                return null;
            }
        }

        public async Task Delete(string endPoint)
        {
            HttpResponseMessage response = await GetHttpClient().DeleteAsync(endPoint);
            if (!response.IsSuccessStatusCode)
            {
                await ThrowHttpResponseMessage(response, endPoint).ConfigureAwait(false);
            }
        }

        private async Task ThrowHttpResponseMessage(HttpResponseMessage response, string endPoint)
        {
            string httpErrors = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Exception innerException = null;
            try
            {
                if (httpErrors != null)
                {
                    Exception apiException = JsonConvert.DeserializeObject<Exception>(httpErrors);
                    innerException = apiException;
                }
            }
            catch (JsonReaderException)
            {
                throw new Exception(httpErrors);
            }

            string message = $"Received {(int)response.StatusCode} ({response.StatusCode}) from {GetHttpClient().BaseAddress}{endPoint}";
            throw new Exception(message, innerException);
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
