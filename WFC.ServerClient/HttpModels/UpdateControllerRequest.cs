using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class UpdateControllerRequest
    {
        [AliasAs("id")]
        public int Id { get; set; }

        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("host")]
        public string Host { get; set; }

        [AliasAs("port")]
        public string Port { get; set; }

        [AliasAs("serialNo")]
        public string SerialNo { get; set; }

        [AliasAs("location")]
        public string Location { get; set; }

        [AliasAs("locationName")]
        public string LocationName { get; set; }

        [AliasAs("openDoorDuration")]
        public int OpenDoorDuration { get; set; }

        [AliasAs("areas")]
        public string Areas { get; set; }

    }
}