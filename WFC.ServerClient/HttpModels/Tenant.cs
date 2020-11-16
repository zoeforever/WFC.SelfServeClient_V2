using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class Tenant
    {
        [AliasAs("id")]
        public int Id { get; set; }

        [AliasAs("idCardNo")]
        public string IdCardNo { get; set; }

        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("nation")]
        public string Nation { get; set; }

        [AliasAs("gender")]
        public string Gender { get; set; }

        [AliasAs("phone")]
        public string Phone { get; set; }

        [AliasAs("nativePlace")]
        public string NativePlace { get; set; }

        [AliasAs("age")]
        public int Age { get; set; }

        [AliasAs("avatar")]
        public string Avatar { get; set; }

        [AliasAs("idCardPhoto")]
        public string IdCardPhoto { get; set; }

        [AliasAs("icCardNo")]
        public string IcCardNo { get; set; }

        [AliasAs("nfcCardNo")]
        public string NfcCardNo { get; set; }

        [AliasAs("qrCodeNo")]
        public string QrCodeNo { get; set; }

        [AliasAs("cardValidFrom")]
        public System.DateTimeOffset CardValidFrom { get; set; }

        [AliasAs("cardValidTo")]
        public System.DateTimeOffset CardValidTo { get; set; }

        [AliasAs("remark")]
        public string Remark { get; set; }

        [AliasAs("inBlackList")]
        public bool InBlackList { get; set; }

        [AliasAs("hendersonTanantId")]
        public string HendersonTanantId { get; set; }

        [AliasAs("hendersonTanantName")]
        public string HendersonTanantName { get; set; }

        [AliasAs("eastFloors")]
        public string EastFloors { get; set; }

        [AliasAs("westFloors")]
        public string WestFloors { get; set; }

        [AliasAs("floorNames")]
        public string FloorNames { get; set; }

        [AliasAs("buildings")]
        public string Buildings { get; set; }

        [AliasAs("buildingNames")]
        public string BuildingNames { get; set; }

        [AliasAs("visitoType")]
        public VisitorType VisitoType { get; set; }

        [AliasAs("numberOfAccess")]
        public int NumberOfAccess { get; set; }

        [AliasAs("credentialId")]
        public string CredentialId { get; set; }

        [AliasAs("codeType")]
        public string CodeType { get; set; }

        [AliasAs("locationJson")]
        public string LocationJson { get; set; }

        [AliasAs("doorStatus")]
        public int DoorStatus { get; set; }

        [AliasAs("isEnable")]
        public int IsEnable { get; set; }

        [AliasAs("visitorStatus")]
        public VisitorStatus VisitorStatus { get; set; }

        [AliasAs("hendersonTenantPersonName")]
        public string HendersonTenantPersonName { get; set; }

        [AliasAs("hendersonTenantPersonPhone")]
        public string HendersonTenantPersonPhone { get; set; }

        [AliasAs("koalaId")]
        public string KoalaId { get; set; }

        [AliasAs("koalaPhotoIds")]
        public string KoalaPhotoIds { get; set; }

        [AliasAs("eastVip")]
        public bool EastVip { get; set; }

        [AliasAs("westVip")]
        public bool WestVip { get; set; }

    }
}