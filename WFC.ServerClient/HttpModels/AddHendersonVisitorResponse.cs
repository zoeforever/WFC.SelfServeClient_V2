using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace WFC.ServerClient
{
    public partial class AddHendersonVisitorResponse
    {
        [AliasAs("statusCode")]
        public string StatusCode { get; set; }

        [AliasAs("message")]
        public string Message { get; set; }

        [AliasAs("result")]
        public List<VisitorResult> Result { get; set; }

    }
}