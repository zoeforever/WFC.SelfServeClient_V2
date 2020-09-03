using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class AddVisitorRequest
    {
        [AliasAs("credentialId")]
        public string CredentialId { get; set; }

        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("idCardNo")]
        public string IdCardNo { get; set; }

        [AliasAs("idCardPhoto")]
        public string IdCardPhoto { get; set; }

        [AliasAs("phone")]
        public string Phone { get; set; }

        [AliasAs("gender")]
        public string Gender { get; set; }

        [AliasAs("nation")]
        public string Nation { get; set; }

        [AliasAs("nativePlace")]
        public string NativePlace { get; set; }

        [AliasAs("hendersonTanantId")]
        public string HendersonTanantId { get; set; }

        [AliasAs("hendersonTenantName")]
        public string HendersonTenantName { get; set; }

        [AliasAs("floors")]
        public string Floors { get; set; }

        [AliasAs("floorNames")]
        public string FloorNames { get; set; }

        [AliasAs("buildings")]
        public string Buildings { get; set; }

        [AliasAs("buildingNames")]
        public string BuildingNames { get; set; }

        [AliasAs("cardValidFrom")]
        public long CardValidFrom { get; set; }

        [AliasAs("cardValidTo")]
        public long CardValidTo { get; set; }

        [AliasAs("hendersonTenantPersonName")]
        public string HendersonTenantPersonName { get; set; }

        [AliasAs("hendersonTenantPersonPhone")]
        public string HendersonTenantPersonPhone { get; set; }

        [AliasAs("numberOfAccess")]
        public int NumberOfAccess { get; set; }

    }
}