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
    public partial interface IFaceApi : IHttpApi
    {
        /// <summary>
        /// 上传两张图像并进行比对
        /// </summary>
        /// <param name="body">Request</param>
        /// <returns>Success</returns>
        [HttpPost("api/v1/Face/compare")]
        ITask<FaceCompareResult> CompareAsync([JsonContent] FaceCompareRequest body);

        /// <summary>
        /// 获取所有摄像头信息
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("api/v1/Face/screens")]
        ITask<List<KoalaScreen>> GetScreensAsync();

    }
}