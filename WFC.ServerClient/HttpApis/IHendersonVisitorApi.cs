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
    public partial interface IHendersonVisitorApi : IHttpApi
    {
        /// <summary>
        /// 通过BuildId获取所有租户信息
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/HendersonVisitor/tenants")]
        ITask<GetTenantsByBuildingIdResponse> GetTenantsByBuildingIdAsync(string buildingId);

        /// <summary>
        /// 添加租户
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [TraceFilter(Enable = false)]
        [HttpPost("api/v1/HendersonVisitor/visitor")]
        ITask<AddHendersonVisitorResponse> AddVisitorAsync([JsonContent] Microsoft.AspNetCore.Http.IFormFile body);

    }
}