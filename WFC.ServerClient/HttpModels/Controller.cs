using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class Controller
    {
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

        [AliasAs("id")]
        public int Id { get; set; }

        [AliasAs("createTime")]
        public long CreateTime { get; set; }

        [AliasAs("createBy")]
        public int CreateBy { get; set; }

        [AliasAs("updateTime")]
        public long UpdateTime { get; set; }

        [AliasAs("updateBy")]
        public int UpdateBy { get; set; }

        [AliasAs("areas")]
        public string Areas { get; set; }

        [AliasAs("antiGoback")]
        public bool AntiGoback { get; set; }

        [AliasAs("cameraIn")]
        public string CameraIn { get; set; }

        [AliasAs("cameraOut")]
        public string CameraOut { get; set; }

    }
}