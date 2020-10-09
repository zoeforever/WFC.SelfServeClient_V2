using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class FaceCompareRequest
    {
        [AliasAs("image1")]
        public string Image1 { get; set; }

        [AliasAs("image2")]
        public string Image2 { get; set; }

    }
}