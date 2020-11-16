using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class HendersonTenant
    {
        [AliasAs("id")]
        public string Id { get; set; }

        [AliasAs("display_name")]
        public string Display_name { get; set; }

        [AliasAs("description")]
        public string Description { get; set; }

        [AliasAs("locations")]
        public List<Location> Locations { get; set; }

        [AliasAs("contacts")]
        public Contact Contacts { get; set; }

        [AliasAs("tenant_admin_group_id")]
        public string Tenant_admin_group_id { get; set; }

        [AliasAs("tenant_code")]
        public int Tenant_code { get; set; }

    }
}