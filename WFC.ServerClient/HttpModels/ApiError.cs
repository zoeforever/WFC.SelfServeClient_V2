using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WFC.ServerClient
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ApiError
    {
        public ApiErrorDetails Error { get; }

        public ApiError(ApiErrorDetails error)
        {
            Error = error;
        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ApiErrorDetails
    {
        /// <summary>
        /// Http status code
        /// </summary>
        public int StatusCode { get; }
        /// <summary>
        /// Internal error code
        /// </summary>
        public int Code { get; }
        /// <summary>
        /// Localized error message
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Error stacks (only in development mode)
        /// </summary>
        public string Stacks { get; }

        public ApiErrorDetails(int statusCode, int code, string message, string stacks)
        {
            StatusCode = statusCode;
            Code = code;
            Message = message;
            Stacks = stacks;
        }
    }
}
