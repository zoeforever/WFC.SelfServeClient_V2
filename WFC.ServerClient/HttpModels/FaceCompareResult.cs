using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class FaceCompareResult
    {
        [AliasAs("face_info_1")]
        public Face_info Face_info_1 { get; set; }

        [AliasAs("face_info_2")]
        public Face_info Face_info_2 { get; set; }

        [AliasAs("same")]
        public string Same { get; set; }

        [AliasAs("score")]
        public double Score { get; set; }

        [AliasAs("thresholds")]
        public Thresholds Thresholds { get; set; }

    }
}