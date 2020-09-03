using Newtonsoft.Json;
using System;
using WebApiClient;

namespace WFC.ServerClient.Utilities
{
    public static class ExceptionHandler
    {
        public static string HandleException(this Exception ex)
        {
            if (ex is HttpStatusFailureException statusFailureException)
            {
                var result = statusFailureException.ResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var apiError = JsonConvert.DeserializeObject<ApiError>(result);
                if (apiError == null)
                    return result;
                else
                    return apiError.Error.Message;
            }
            else
            {
                return ex.Message;
            }
            //catch (HttpStatusFailureException ex)
            //{
            //    var resutl = await ex.ResponseMessage.Content.ReadAsStringAsync();
            //}
        }
    }
}
