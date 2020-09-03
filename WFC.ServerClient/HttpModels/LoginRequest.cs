using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class LoginRequest
    {
        [AliasAs("userName")]
        [Required(AllowEmptyStrings = true)]
        public string UserName { get; set; }

        [AliasAs("password")]
        [Required(AllowEmptyStrings = true)]
        public string Password { get; set; }

    }
}