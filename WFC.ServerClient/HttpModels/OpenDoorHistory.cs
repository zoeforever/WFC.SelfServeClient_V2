using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class OpenDoorHistory
    {
        [AliasAs("historyId")]
        public int HistoryId { get; set; }

        [AliasAs("location")]
        public string Location { get; set; }

        [AliasAs("locationName")]
        public string LocationName { get; set; }

        [AliasAs("openDoorDateTime")]
        public System.DateTimeOffset OpenDoorDateTime { get; set; }

        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("companyName")]
        public string CompanyName { get; set; }

        [AliasAs("visitorType")]
        public string VisitorType { get; set; }

        [AliasAs("eventType")]
        public string EventType { get; set; }

        [AliasAs("cardNo")]
        public string CardNo { get; set; }

        [AliasAs("controllerName")]
        public string ControllerName { get; set; }

    }
}