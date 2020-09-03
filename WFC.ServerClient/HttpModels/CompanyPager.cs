using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class CompanyPager
    {
        [AliasAs("count")]
        public int Count { get; set; }

        [AliasAs("items")]
        public List<Company> Items { get; set; }

    }
}