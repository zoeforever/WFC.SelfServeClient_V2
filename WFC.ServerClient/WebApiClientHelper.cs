using System;
using System.Collections.Generic;
using System.Text;
using WebApiClient;

namespace WFC.ServerClient
{
    public class WebApiClientHelper
    {
        public static string Jwt { get; set; }
        public static SystemUser Current { get; set; }
    }
}
