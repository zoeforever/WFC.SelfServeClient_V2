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
    public partial interface ISystemUserRoleApi : IHttpApi
    {
        /// <summary>
        /// 查询所有用户角色
        /// </summary>
        /// <param name="name">角色名称</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/SystemUserRole")]
        ITask<List<SystemUserRole>> GetAllUserRolesAsync(string name);

        /// <summary>
        /// 增加用户角色
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/SystemUserRole")]
        ITask<int> AddUserRoleAsync([Required] [JsonContent] AddSystemUserRoleRequest body);

        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/v1/SystemUserRole")]
        ITask<HttpResponseMessage> UpdateUserRoleAsync([Required] [JsonContent] SystemUserRole body);

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/v1/SystemUserRole/{id}")]
        ITask<HttpResponseMessage> DeleteUserRoleAsync([Required] int id);

    }
}