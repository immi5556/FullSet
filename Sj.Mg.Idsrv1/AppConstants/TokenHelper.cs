using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConstants
{
    public class TokenHelper
    {
        public static JObject DecodeAndWrite(string token)
        {
            try
            {
                var parts = token.Split('.');

                string partToConvert = parts[1];
                partToConvert = partToConvert.Replace('-', '+');
                partToConvert = partToConvert.Replace('_', '/');
                switch (partToConvert.Length % 4)
                {
                    case 0:
                        break;
                    case 2:
                        partToConvert += "==";
                        break;
                    case 3:
                        partToConvert += "=";
                        break;
                    default:
                        break;
                }

                var partAsBytes = Convert.FromBase64String(partToConvert);
                var partAsUTF8String = Encoding.UTF8.GetString(partAsBytes, 0, partAsBytes.Count());

                // Json .NET
                var jwt = JObject.Parse(partAsUTF8String);

                // Write to output
                Debug.Write(jwt.ToString());
                return jwt;
            }
            catch (Exception ex)
            {
                // something went wrong
                Debug.Write(ex.Message);
            }
            return null;
        }

        public static string CreateJwt(string payload)
        {
            var hdr = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { alg = "HS256", typ = "JWT" })));
            var pld = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(payload));
            return hdr + "." + pld;
        }
    }
}
