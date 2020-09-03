using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class UpdateCompanyRequest
    {
        [AliasAs("id")]
        public int Id { get; set; }

        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("nation")]
        public string Nation { get; set; }

        [AliasAs("floor")]
        public string Floor { get; set; }

        [AliasAs("phone")]
        public string Phone { get; set; }

        [AliasAs("owner")]
        public string Owner { get; set; }

        [AliasAs("ownerPhone")]
        public string OwnerPhone { get; set; }

        [AliasAs("email")]
        public string Email { get; set; }

        [AliasAs("corpType")]
        public string CorpType { get; set; }

        [AliasAs("visitorRoleId")]
        public int VisitorRoleId { get; set; }

    }
}