using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class AddTenantRequest
    {
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
        public long CardValidFrom { get; set; }

        [AliasAs("cardValidTo")]
        public long CardValidTo { get; set; }

        [AliasAs("remark")]
        public string Remark { get; set; }

        [AliasAs("inBlackList")]
        public bool InBlackList { get; set; }

        [AliasAs("hendersonTanantId")]
        public string HendersonTanantId { get; set; }

        [AliasAs("hendersonTanantName")]
        public string HendersonTanantName { get; set; }

        [AliasAs("floors")]
        public string Floors { get; set; }

        [AliasAs("floorNames")]
        public string FloorNames { get; set; }

        [AliasAs("buildings")]
        public string Buildings { get; set; }

        [AliasAs("buildingNames")]
        public string BuildingNames { get; set; }

    }
}