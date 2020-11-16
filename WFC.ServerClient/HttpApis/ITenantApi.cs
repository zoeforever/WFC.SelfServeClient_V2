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
    public partial interface ITenantApi : IHttpApi
    {
        /// <summary>
        /// 查询所有租户信息
        /// </summary>
        /// <param name="visitorType">类型（租户、访客）</param>
        /// <param name="name">姓名</param>
        /// <param name="phone">电话</param>
        /// <param name="idCardNo">身份证</param>
        /// <param name="icCardNo">IC卡号</param>
        /// <param name="corp">公司</param>
        /// <param name="inBlackList">是否是黑名单</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/Tenant")]
        ITask<TenantPager> GetAllTenantsAsync(string visitorType, string name, string phone, string idCardNo, string icCardNo, string corp, string inBlackList, int? page, int? pageSize);

        /// <summary>
        /// 增加租户信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/Tenant")]
        ITask<AddTenantResponse> AddTenantAsync([Required] [JsonContent] AddTenantRequest body);

        /// <summary>
        /// 更新租户信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/v1/Tenant")]
        ITask<UpdateTenantResponse> UpdateTenantAsync([Required] [JsonContent] UpdateTenantRequest body);

        /// <summary>
        /// 根据公司名称批量修改租户信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/Tenant/company")]
        ITask<HttpResponseMessage> UpdateTenantsByCompanyAsync([Required] [JsonContent] UpdateTenantsByCompanyRequest body);

        /// <summary>
        /// 删除租户信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/v1/Tenant/del")]
        ITask<HttpResponseMessage> DeleteTenantAsync([Required] [JsonContent] DeleteTenantRequest body);

        /// <summary>
        /// 批量删除租户
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/Tenant/delMore")]
        ITask<HttpResponseMessage> DeleteTenantsAsync([JsonContent] DeleteTenantsRequest body);

        /// <summary>
        /// 给访客添加Ic卡
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/Tenant/iccard")]
        ITask<HttpResponseMessage> UpdateTenantIcCardAsync([Required] [JsonContent] UpdateTenantIcCardRequest body);

    }
}