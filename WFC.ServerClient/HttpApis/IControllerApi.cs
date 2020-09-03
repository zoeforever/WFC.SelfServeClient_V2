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
    public partial interface IControllerApi : IHttpApi
    {
        /// <summary>
        /// 查询所有控制器信息
        /// </summary>
        /// <param name="name">控制器名</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/Controller")]
        ITask<ControllerPager> GetAllControllersAsync(string name, int? page, int? pageSize);

        /// <summary>
        /// 增加控制器信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/Controller")]
        ITask<int> AddControllerAsync([Required] [JsonContent] AddControllerRequest body);

        /// <summary>
        /// 更新控制器信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/v1/Controller")]
        ITask<HttpResponseMessage> UpdateControllerAsync([Required] [JsonContent] UpdateControllerRequest body);

        /// <summary>
        /// 删除控制器信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/v1/Controller/{id}")]
        ITask<HttpResponseMessage> DeleteControllerAsync([Required] int id);

    }
}