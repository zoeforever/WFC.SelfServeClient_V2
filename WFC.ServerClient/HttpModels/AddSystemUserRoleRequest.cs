using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class AddSystemUserRoleRequest
    {
        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("permissions")]
        public List<int> Permissions { get; set; }

    }
}