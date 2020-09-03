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
    [TraceFilter]
    public partial interface ISystemUserApi : IHttpApi
    {
        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/SystemUser")]
        ITask<List<SystemUser>> GetAllUsersAsync(string name);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/SystemUser")]
        ITask<int> AddUserAsync([Required] [JsonContent] AddSystemUserRequest body);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/v1/SystemUser")]
        ITask<HttpResponseMessage> UpdateUserAsync([Required] [JsonContent] SystemUser body);

        /// <summary>
        /// 根据用户名查询用户信息
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/SystemUser/{name}")]
        ITask<SystemUser> GetUserByNameAsync([Required] string name);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/v1/SystemUser/{userId}")]
        ITask<HttpResponseMessage> DeleteUserAsync([Required] int userId);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/SystemUser/password/change")]
        ITask<bool> UpdatePasswordAsync([JsonContent] UpdateSystemUserPasswordRequest body);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/SystemUser/password/reset")]
        ITask<HttpResponseMessage> ResetPasswordAsync([JsonContent] ResetSystemUserPasswordRequest body);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/SystemUser/login")]
        ITask<LoginResponse> LoginAsync([Required] [JsonContent] LoginRequest body);

    }
}