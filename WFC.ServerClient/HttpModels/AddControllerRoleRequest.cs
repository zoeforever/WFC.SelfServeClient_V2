using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class AddControllerRoleRequest
    {
        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("controllerIds")]
        public List<int> ControllerIds { get; set; }

    }
}