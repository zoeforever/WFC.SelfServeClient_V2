using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class Face_info
    {
        [AliasAs("rect")]
        public Rect Rect { get; set; }

        [AliasAs("quality")]
        public double Quality { get; set; }

        [AliasAs("brightness")]
        public double Brightness { get; set; }

        [AliasAs("std_deviation")]
        public double Std_deviation { get; set; }

    }
}