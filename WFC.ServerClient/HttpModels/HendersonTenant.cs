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
        public List<Contact> Contacts { get; set; }

    }
}