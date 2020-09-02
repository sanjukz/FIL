using FIL.Contracts.Models.Integrations.HttpHelpers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.HttpHelpers
{
    public class HttpWebRequestHelpers<T>
    {
        public static async Task<WebRequest> GetWebRequest(string url, string method, IRequestCreateOptions<T> requestCreateOptions, bool useBearer = false)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                var content = string.Empty;
                if (requestCreateOptions.Content != null)
                {
                    content = Mapper<T>.MapObjectToJsonString(requestCreateOptions.Content);
                }

                request.Method = method;
                request.ContentLength = content.Length;
                request.ContentType = requestCreateOptions.ContentType;
                if (!string.IsNullOrWhiteSpace(request.Accept))
                {
                    request.Accept = requestCreateOptions.Accept;
                }
                if (requestCreateOptions.Content != null)
                {
                    foreach (var header in requestCreateOptions.Header)
                    {
                        request.Headers.Add(GetAuthorizationHeader(header.Value, useBearer));
                    }
                }
                request.UserAgent = requestCreateOptions.UserAgent;

                if (!string.IsNullOrWhiteSpace(content))
                {
                    StreamWriter myWriter = null;
                    myWriter = new StreamWriter(await request.GetRequestStreamAsync().ConfigureAwait(false));
                    myWriter.Write(content);
                    myWriter.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return request;
        }

        private static string GetAuthorizationHeader(string headerValue, bool useBearer = false)
        {
            if (!useBearer)
            {
                byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(headerValue);
                return string.Format("Authorization: Basic {0}", Convert.ToBase64String(encbuff));
            }
            else
            {
                return "Authorization: Bearer " + headerValue;
            }
        }

        public static async Task<IHttpResponse> ExecuteWebRequest(WebRequest webRequest)
        {
            IHttpResponse _response = new HttpResponse();
            try
            {
                using (var response = await webRequest.GetResponseAsync().ConfigureAwait(false))
                {
                    _response.Response = ReadStream(response.GetResponseStream());
                    return _response;
                }
            }
            catch (WebException webException)
            {
                using (StreamReader responseStream = new StreamReader(webException.Response.GetResponseStream()))
                {
                    string exception = responseStream.ReadToEnd();
                    Error error = new Error();
                    error = Mapper<Error>.MapJsonStringToObject(exception);
                    if (!string.IsNullOrWhiteSpace(error.Code))
                    {
                        _response.Code = error.Code;
                        _response.Message = error.Message;
                    }
                    responseStream.Close();
                }
                if (webException.Response != null && string.IsNullOrWhiteSpace(_response.Code))
                {
                    var statusCode = ((HttpWebResponse)webException.Response).StatusCode;
                    var statusDescription = ((HttpWebResponse)webException.Response).StatusDescription;
                    _response.Code = statusCode.ToString();
                    _response.Message = statusDescription.ToString();
                }
                _response.WebException = webException;
                return _response;
            }
        }

        private static string ReadStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}