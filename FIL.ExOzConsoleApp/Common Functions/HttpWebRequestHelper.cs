using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FIL.ExOzConsoleApp
{
    public static class HttpWebRequestHelper
    {
        public static string ExOz_authToken = "Basic OTE3NjphM2xoZW05dmJtZGhTM2xoZW05dmJtZGhJRUYxYzNSeVlXeHBZU0JRZEhrZ1RIUms=";
        public static string ExOz_EndPoint = "http://kyazoonga.apitest.com.au/en/api/v2";
        public static string ExOz_WebRequestGet(string Api)
        {
            string endpoint = ExOz_EndPoint + "/" + Api;
            var webRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            webRequest.Headers.Add("Authorization", ExOz_authToken);
            webRequest.Method = "GET";
            string strResponse = "";

            try
            {
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
                {
                    var reader = new StreamReader(webResponse.GetResponseStream());
                    strResponse = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                strResponse = "0";
            }
            return strResponse;
        }
    }
}
