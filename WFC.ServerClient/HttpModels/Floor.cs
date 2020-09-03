using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class Floor
    {
        [AliasAs("id")]
        public string Id { get; set; }

        [AliasAs("display_name")]
        public string Display_name { get; set; }

        [AliasAs("idx")]
        public int Idx { get; set; }

    }
}