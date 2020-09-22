using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC.SelfServeClient.Models
{
    public class AddVisitorResponse
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public List<VisitorResult> Result { get; set; }
    }

    public class VisitorResult
    {
        public string CredentialId { get; set; }
        public string Status { get; set; }
    }
}
