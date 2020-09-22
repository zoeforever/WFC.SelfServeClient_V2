using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WFC.SelfServeClient.Helper
{
    public class HttpClientHelper
    {
        public static RestRequest GetRestRequest(string url, Method method)
        {
            RestRequest request = new RestRequest(url, method);
            return request;
        }

        public static T RestPostFile<T>(string url, string bearAuthToken, Dictionary<string, string> fields, Dictionary<string, string> filePaths, int timeout = -1) where T : class
        {
            try
            {
                RestClient client = new RestClient();
                client.Timeout = timeout;
                client.ReadWriteTimeout = timeout;
                var request = GetRestRequest(url, Method.POST);
                request.AddHeader("Authorization", "Bearer " + bearAuthToken);

                if (filePaths != null)
                    foreach (var item in filePaths)
                    {
                        request.AddFile(item.Key, item.Value);
                    }

                if (fields != null)
                    foreach (var item in fields)
                    {
                        request.AddParameter(item.Key, item.Value);
                    }

                var response = client.Execute(request);
                return JsonHelper.FromJson<T>(response.Content);
            }
            catch (Exception ex)
            {
                return null;
                Logger.Error(ex.ToString());
            }
        }

    }
}
