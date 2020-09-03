using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class Space
    {
        [AliasAs("id")]
        public string Id { get; set; }

        [AliasAs("display_name")]
        public string Display_name { get; set; }

        [AliasAs("space")]
        public Space Space1 { get; set; }

    }
}