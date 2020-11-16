using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class KoalaScreen
    {
        [AliasAs("app_version")]
        public string App_version { get; set; }

        [AliasAs("box_address")]
        public string Box_address { get; set; }

        [AliasAs("box_heartbeat")]
        public int Box_heartbeat { get; set; }

        [AliasAs("box_id")]
        public int Box_id { get; set; }

        [AliasAs("box_status")]
        public string Box_status { get; set; }

        [AliasAs("box_token")]
        public string Box_token { get; set; }

        [AliasAs("camera_address")]
        public string Camera_address { get; set; }

        [AliasAs("camera_name")]
        public string Camera_name { get; set; }

        [AliasAs("camera_position")]
        public string Camera_position { get; set; }

        [AliasAs("camera_status")]
        public string Camera_status { get; set; }

        [AliasAs("description")]
        public string Description { get; set; }

        [AliasAs("group_id")]
        public string Group_id { get; set; }

        [AliasAs("group_name")]
        public string Group_name { get; set; }

        [AliasAs("id")]
        public int Id { get; set; }

        [AliasAs("is_select")]
        public int Is_select { get; set; }

        [AliasAs("network_switcher")]
        public string Network_switcher { get; set; }

        [AliasAs("network_switcher_drive")]
        public int Network_switcher_drive { get; set; }

        [AliasAs("network_switcher_status")]
        public string Network_switcher_status { get; set; }

        [AliasAs("network_switcher_token")]
        public string Network_switcher_token { get; set; }

        [AliasAs("screen_token")]
        public string Screen_token { get; set; }

        [AliasAs("server_time")]
        public double Server_time { get; set; }

        [AliasAs("type")]
        public int Type { get; set; }

    }
}