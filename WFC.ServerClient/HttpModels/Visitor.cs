using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class Visitor
    {
        [AliasAs("name")]
        public string Name { get; set; }

        [AliasAs("idCardNo")]
        public string IdCardNo { get; set; }

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

        [AliasAs("remark")]
        public string Remark { get; set; }

        [AliasAs("corpId")]
        public int CorpId { get; set; }

        [AliasAs("accessMedia")]
        public AccessMedia AccessMedia { get; set; }

        [AliasAs("cardNo")]
        public string CardNo { get; set; }

        [AliasAs("cardValidFrom")]
        public System.DateTimeOffset CardValidFrom { get; set; }

        [AliasAs("cardValidTo")]
        public System.DateTimeOffset CardValidTo { get; set; }

        [AliasAs("visitorType")]
        public VisitorType VisitorType { get; set; }

        [AliasAs("controllerRoleId")]
        public int ControllerRoleId { get; set; }

    }
}