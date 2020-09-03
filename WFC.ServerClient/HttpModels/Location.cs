using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class Location
    {
        [AliasAs("floor")]
        public Floor Floor { get; set; }

        [AliasAs("building")]
        public Building Building { get; set; }

        [AliasAs("campus")]
        public Campus Campus { get; set; }

        [AliasAs("space")]
        public Space Space { get; set; }

    }
}