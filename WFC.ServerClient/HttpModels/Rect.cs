using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class Rect
    {
        [AliasAs("left")]
        public int Left { get; set; }

        [AliasAs("top")]
        public int Top { get; set; }

        [AliasAs("right")]
        public int Right { get; set; }

        [AliasAs("bottom")]
        public int Bottom { get; set; }

    }
}