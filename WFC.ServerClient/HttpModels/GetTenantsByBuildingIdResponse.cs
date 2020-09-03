using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class GetTenantsByBuildingIdResponse
    {
        [AliasAs("status_code")]
        public string Status_code { get; set; }

        [AliasAs("msg")]
        public string Msg { get; set; }

        [AliasAs("result")]
        public List<HendersonTenant> Result { get; set; }

    }
}