using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class DeleteTenantsRequest
    {
        [AliasAs("ids")]
        public List<TenantItem> Ids { get; set; }

    }
}