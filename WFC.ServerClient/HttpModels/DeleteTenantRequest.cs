using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class DeleteTenantRequest
    {
        [AliasAs("tenantId")]
        public int TenantId { get; set; }

        [AliasAs("koalaId")]
        public string KoalaId { get; set; }

    }
}