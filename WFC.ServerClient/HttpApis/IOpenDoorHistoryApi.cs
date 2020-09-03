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
    public partial interface IOpenDoorHistoryApi : IHttpApi
    {
        /// <summary>
        /// 查询所有刷卡记录
        /// </summary>
        /// <param name="fromTime">开始时间</param>
        /// <param name="toTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>Success</returns>
        [HttpGet("api/v1/OpenDoorHistory")]
        ITask<OpenDoorHistoryPager> GetAllOpenDoorHistoryAsync(long? fromTime, long? toTime, int? page, int? pageSize);

    }
}