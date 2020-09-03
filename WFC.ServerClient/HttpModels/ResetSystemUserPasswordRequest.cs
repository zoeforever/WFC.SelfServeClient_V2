using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class ResetSystemUserPasswordRequest
    {
        [AliasAs("name")]
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        [AliasAs("newPassword")]
        [Required(AllowEmptyStrings = true)]
        public string NewPassword { get; set; }

    }
}