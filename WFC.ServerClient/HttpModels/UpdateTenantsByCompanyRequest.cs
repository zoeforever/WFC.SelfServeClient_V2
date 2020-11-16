using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class UpdateTenantsByCompanyRequest
    {
        [AliasAs("companyName")]
        public string CompanyName { get; set; }

        [AliasAs("eastFloors")]
        public string EastFloors { get; set; }

        [AliasAs("westFloors")]
        public string WestFloors { get; set; }

        [AliasAs("areas")]
        public string Areas { get; set; }

    }
}