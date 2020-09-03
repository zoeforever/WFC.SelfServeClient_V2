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
    public partial interface IVisitorApi : IHttpApi
    {
        /// <summary>
        /// 查询所有访客信息
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/Visitor")]
        ITask<VisitorPager> GetAllVisitorsAsync(string name, int? page, int? pageSize);

        /// <summary>
        /// 增加访客信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/Visitor")]
        ITask<int> AddPendingVisitorAsync([Required] [JsonContent] AddVisitorRequest body);

        /// <summary>
        /// 更新访客信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/v1/Visitor")]
        ITask<HttpResponseMessage> UpdateVisitorAsync([Required] [JsonContent] UpdateVisitorRequest body);

        /// <summary>
        /// 删除访客信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/v1/Visitor/{id}")]
        ITask<HttpResponseMessage> DeleteVisitorAsync([Required] int id);

        /// <summary>
        /// 通过名称或者身份证号码查询现有访客
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="idCardNo">Id Card No</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/Visitor/NameOrCardNo")]
        ITask<TenantPager> GetVisitorByNameAndIdCardNoAsync(string name, string idCardNo);

    }
}