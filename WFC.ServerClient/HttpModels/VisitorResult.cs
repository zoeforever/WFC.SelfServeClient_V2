using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class VisitorResult
    {
        [AliasAs("credentialId")]
        public string CredentialId { get; set; }

        [AliasAs("status")]
        public string Status { get; set; }

    }
}