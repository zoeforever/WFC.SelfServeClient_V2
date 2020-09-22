using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC.SelfServeClient.Helper
{
    public class JsonHelper
    {
        public static T FromJson<T>(string json)
        {
            try
            {
                T obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return default(T);
        }

        public static string ToJson(object obj, bool ignoreNull = false)
        {
            string message;
            if (ignoreNull)
            {
                var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

                message = Newtonsoft.Json.JsonConvert.SerializeObject(obj, jsonSetting);
            }
            else
            {
                message = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            return message;
        }

        public static T FromJsonString<T>(string json)
        {
            var obj = JObject.Parse(json);
            return obj.ToObject<T>();
        }



    }
}

