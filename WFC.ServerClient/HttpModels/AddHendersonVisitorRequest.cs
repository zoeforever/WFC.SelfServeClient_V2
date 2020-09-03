using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class AddHendersonVisitorRequest
    {
        [AliasAs("visitorName")]
        [Required(AllowEmptyStrings = true)]
        public string VisitorName { get; set; }

        [AliasAs("phoneNumber")]
        [Required(AllowEmptyStrings = true)]
        public string PhoneNumber { get; set; }

        [AliasAs("visitorPhoto")]
        public string VisitorPhoto { get; set; }

        [AliasAs("startTime")]
        public long StartTime { get; set; }

        [AliasAs("endTime")]
        public long EndTime { get; set; }

        [AliasAs("numberOfAccess")]
        public int NumberOfAccess { get; set; }

        [AliasAs("idCardNumber")]
        [Required(AllowEmptyStrings = true)]
        public string IdCardNumber { get; set; }

        [AliasAs("tenantId")]
        [Required(AllowEmptyStrings = true)]
        public string TenantId { get; set; }

        [AliasAs("tenantContactPerson")]
        [Required(AllowEmptyStrings = true)]
        public string TenantContactPerson { get; set; }

        [AliasAs("visitorType")]
        public VisitorType VisitorType { get; set; }

    }
}