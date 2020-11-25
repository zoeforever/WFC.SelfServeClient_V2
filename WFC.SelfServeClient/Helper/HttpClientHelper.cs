using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WFC.ServerClient;

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

                var response = client.Execute(request); if (response.StatusCode != HttpStatusCode.OK)
                {
                    var apiError = JsonConvert.DeserializeObject<ApiError>(response.Content);
                    if (apiError == null)
                        return JsonHelper.FromJson<T>(response.Content);
                    else
                        throw new Exception(apiError.Error.Message);
                }
                return JsonHelper.FromJson<T>(response.Content);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw;
            }
        }

    }
}
