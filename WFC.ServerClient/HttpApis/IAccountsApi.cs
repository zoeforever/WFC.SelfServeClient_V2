using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;
using WebApiClient.DataAnnotations;
using WebApiClient.Parameterables;

namespace WFC.ServerClient
{
    //[TraceFilter]
    public partial interface IAccountsApi : IHttpApi
    {
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("v1/account/authorize")]
        ITask<HttpResponseMessage> AccountsAsync([JsonContent] HendersonLoginRequest body);
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("v1/account/verify-code")]
        ITask<VerifyCodeResponse> VerifyCodeAsync([JsonContent] VerifyCodeRequest body);
        /// <param name="refreshToken"></param>
        /// <returns>Success</returns>
        [HttpPost("v1/account/refresh-token")]
        ITask<GrantRefreshTokenResponse> RefreshAsync([Required][JsonContent] GrantRefreshTokenRequest refreshToken);
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>Success</returns>
        [HttpPost("v1/account/logout")]
        ITask<HttpResponseMessage> LogoutAsync();
    }
}