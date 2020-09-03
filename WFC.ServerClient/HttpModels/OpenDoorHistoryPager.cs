using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class OpenDoorHistoryPager
    {
        [AliasAs("count")]
        public int Count { get; set; }

        [AliasAs("items")]
        public List<OpenDoorHistory> Items { get; set; }

    }
}