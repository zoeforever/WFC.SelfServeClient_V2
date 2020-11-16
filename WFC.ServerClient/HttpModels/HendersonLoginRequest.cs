using Newtonsoft.Json;
using System.Collections.Generic;
using WebApiClient.DataAnnotations;

namespace WFC.ServerClient
{
    /// <summary>
    /// 
    /// </summary>
    public partial class HendersonLoginRequest
    {
        /// <summary>
        /// 
        /// </summary>

        [AliasAs("area_code")]
        public string AreaCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [AliasAs("phone_number")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [AliasAs("organization_identifier")]
        public string OrganizationIdentifier { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HendersonLoginResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("status_code")]
        public string StatusCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("msg")]
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("result")]
        public List<string> Result { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class VerifyCodeRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [AliasAs("area_code")]
        public string AreaCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [AliasAs("phone_number")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [AliasAs("verify_code")]
        public string VerifyCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VerifyCodeResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("status_code")]
        public string StatusCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("msg")]
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("result")]
        public List<VerifiedUser> Result { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VerifiedUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string first_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string last_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string display_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> policies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string refresh_token { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GrantRefreshTokenRequest
    {
        /// <summary>
        /// refresh_token
        /// </summary>
        [AliasAs("grant_type")]
        public string GrantType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [AliasAs("refresh_token")]
        public string RefreshToken { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GrantRefreshToken
    {
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string refresh_token { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GrantRefreshTokenResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string status_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<GrantRefreshToken> result { get; set; }
    }

}