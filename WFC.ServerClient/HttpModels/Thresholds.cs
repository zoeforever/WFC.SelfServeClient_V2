using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class Thresholds
    {
        [AliasAs("e3")]
        public double E3 { get; set; }

        [AliasAs("e4")]
        public double E4 { get; set; }

        [AliasAs("e5")]
        public double E5 { get; set; }

        [AliasAs("e6")]
        public double E6 { get; set; }

        [AliasAs("recognizing")]
        public int Recognizing { get; set; }

        [AliasAs("stranger")]
        public int Stranger { get; set; }

        [AliasAs("verify")]
        public int Verify { get; set; }

        [AliasAs("gate")]
        public int Gate { get; set; }

    }
}