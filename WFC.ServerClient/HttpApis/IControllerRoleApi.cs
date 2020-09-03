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
    public partial interface IControllerRoleApi : IHttpApi
    {
        /// <summary>
        /// 查询所有用户角色
        /// </summary>
        /// <param name="name">角色名称</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/ControllerRole")]
        ITask<List<ControllerRole>> GetAllControllerRolesAsync(string name);

        /// <summary>
        /// 增加控制器角色
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/ControllerRole")]
        ITask<int> AddControllerRoleAsync([Required] [JsonContent] AddControllerRoleRequest body);

        /// <summary>
        /// 更新控制器角色
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/v1/ControllerRole")]
        ITask<HttpResponseMessage> UpdateControllerRoleAsync([Required] [JsonContent] UpdateControllerRoleRequest body);

        /// <summary>
        /// 删除控制器角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/v1/ControllerRole/{id}")]
        ITask<HttpResponseMessage> DeleteControllerRoleAsync([Required] int id);

    }
}