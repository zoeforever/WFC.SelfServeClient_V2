using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class UpdateTenantIcCardRequest
    {
        [AliasAs("id")]
        public int Id { get; set; }

        [AliasAs("icCardNo")]
        public string IcCardNo { get; set; }

        [AliasAs("areas")]
        public string Areas { get; set; }

    }
}