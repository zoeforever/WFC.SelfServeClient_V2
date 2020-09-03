using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class LoginResponse
    {
        [AliasAs("token")]
        public string Token { get; set; }

        [AliasAs("id")]
        public int Id { get; set; }

        [AliasAs("userName")]
        public string UserName { get; set; }

        [AliasAs("roles")]
        public List<int> Roles { get; set; }

    }
}