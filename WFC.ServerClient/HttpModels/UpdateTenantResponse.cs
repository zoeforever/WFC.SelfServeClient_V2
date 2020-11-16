using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class UpdateTenantResponse
    {
        [AliasAs("code")]
        public int Code { get; set; }

        [AliasAs("message")]
        public string Message { get; set; }

    }
}