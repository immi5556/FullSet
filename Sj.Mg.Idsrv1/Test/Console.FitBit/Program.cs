using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Console.FitBit
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetAccTokenByRefreshToken();
            GetAccData();
        }

        static void FitBit()
        {
            WebRequest request = WebRequest.Create(GetCode());
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + "";
        }

        static string GetCode()
        {
            string url = "https://www.fitbit.com/oauth2/authorize?" + 
                "response_type=code&client_id=228JJF" + 
                "&redirect_uri=http%3A%2F%2Flocalhost%3A54728%2Fapi%2Fvalues" + 
                "&scope=activity%20heartrate%20location%20nutrition%20profile%20settings%20sleep%20social%20weight" + 
                "&expires_in=604800";
            return url;
        }

        static string GetAccessToken()
        {
            string url = "https://api.fitbit.com/oauth2/token?" + 
                "grant_type=refresh_token" + 
                "&refresh_token=4593b06f06b5832973ae7b927b7d8e6022a6951cd252a71d5b24b47126da53ff";
            return url;
        }

        static string GetAccData()
        {
            string url = "https://api.fitbit.com/1/user/-/profile.json";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiI1UTczNzMiLCJhdWQiOiIyMjhKSkYiLCJpc3MiOiJGaXRiaXQiLCJ0eXAiOiJhY2Nlc3NfdG9rZW4iLCJzY29wZXMiOiJyc29jIHJzZXQgcmFjdCBybG9jIHJ3ZWkgcmhyIHJwcm8gcm51dCByc2xlIiwiZXhwIjoxNDk1NjczMzM4LCJpYXQiOjE0OTU2NDQ1Mzh9.sT7wMh6BHsCspRLj7KfQXVyX0gLxH6rs8q4nPTGaqWw";
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, Encoding.UTF8);
            var tt = reader.ReadToEnd();
            Newtonsoft.Json.Linq.JObject patient = Newtonsoft.Json.Linq.JObject.Parse(tt);
            return null;
        }

        static string GetAccTokenByRefreshToken()
        {
            string rtkn = "0fe8b9f7c90b789d0bf3a3f2751ce0bcc10bab1c09acfd4e55edd5bdfd88a6c2";
            string url = "https://api.fitbit.com/oauth2/token?grant_type=refresh_token&refresh_token=" + rtkn;
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = "Basic MjI4SkpGOjNjMjY4YTllYzFiYTMzNDJkNTEyNzIyMDkzMDc5NGYx";
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, Encoding.UTF8);
            var tt = reader.ReadToEnd();
            Newtonsoft.Json.Linq.JObject patient = Newtonsoft.Json.Linq.JObject.Parse(tt);
            return null;
        }
    }
}
