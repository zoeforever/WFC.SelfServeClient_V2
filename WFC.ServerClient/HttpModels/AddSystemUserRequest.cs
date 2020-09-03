using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class AddSystemUserRequest
    {
        [AliasAs("userName")]
        public string UserName { get; set; }

        [AliasAs("password")]
        public string Password { get; set; }

        [AliasAs("status")]
        public SystemUserStatus Status { get; set; }

        [AliasAs("roleIds")]
        public List<int> RoleIds { get; set; }

    }
}