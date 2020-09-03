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
    public partial interface ICompanyApi : IHttpApi
    {
        /// <summary>
        /// 查询所有公司信息
        /// </summary>
        /// <param name="name">公司名</param>
        /// <param name="phone">电话</param>
        /// <param name="owner">负责人</param>
        /// <param name="ownerPhone">负责人电话</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/Company")]
        ITask<CompanyPager> GetAllCompaniesAsync(string name, string phone, string owner, string ownerPhone, int? page, int? pageSize);

        /// <summary>
        /// 增加公司信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/Company")]
        ITask<int> AddCompanyAsync([Required] [JsonContent] AddCompanyRequest body);

        /// <summary>
        /// 更新公司信息
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/v1/Company")]
        ITask<HttpResponseMessage> UpdateCompanyAsync([Required] [JsonContent] UpdateCompanyRequest body);

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/v1/Company/{id}")]
        ITask<HttpResponseMessage> DeleteCompanyAsync([Required] int id);

    }
}