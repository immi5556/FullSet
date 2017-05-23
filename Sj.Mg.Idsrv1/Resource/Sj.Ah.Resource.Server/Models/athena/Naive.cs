//    Copyright 2014 athenahealth, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License"); you
//   may not use this file except in compliance with the License.  You
//   may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
//   implied.  See the License for the specific language governing
//   permissions and limitations under the License.

using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
//using System.Json;
namespace Sj.Ah.Resource.Server.athena
{
    public class Naive
    {


        static void CreatePatient()
        {
            //string url = "https://api.athenahealth.com/preview1/195900/patients";

            string key = "cezsucv9zy2k3uu464qmkf4z";
            string secret = "bHZfSdakTnCNmVH";
            string version = "preview1";
            string practiceid = "195900";

            string baseUrl = "https://api.athenahealth.com/";


            // Easier to keep track of OAuth prefixes
            Dictionary<string, string> auth_prefixes = new Dictionary<string, string>()
      {
        {"v1", "/oauth"},
        {"preview1", "/oauthpreview"},
        {"openpreview1", "/oauthopenpreview"},
      };


            // Basic access authentication
            Dictionary<string, string> parameters = new Dictionary<string, string>()
      {
        {"grant_type", "client_credentials"},
      };

            // Create and set up a request
            WebRequest request = WebRequest.Create(Utils.PathJoin(baseUrl, auth_prefixes[version], "/token"));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // Make sure to add the Authorization header
            string auth = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(key + ":" + secret));
            request.Headers["Authorization"] = "Basic " + auth;

            // Encode the parameters, convert it to bytes (because that's how the streams want it)
            string encoded = Utils.UrlEncode(parameters);
            byte[] content = Encoding.UTF8.GetBytes(encoded);

            // Write the parameters to the body
            Stream writer = request.GetRequestStream();
            writer.Write(content, 0, content.Length);
            writer.Close();

            // Get the response, read it out, and decode it
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, Encoding.UTF8);
            //JsonValue authorization = JsonValue.Parse(reader.ReadToEnd());
            //Object authorization = Newtonsoft.Json.JsonConvert.DeserializeObject(reader.ReadToEnd());
            Newtonsoft.Json.Linq.JObject authorization = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd());
            // Make sure to grab the token!
            string token = authorization["access_token"].ToString();
            Console.WriteLine(token);

        }

        static void GetDepartment(string baseUrl, string token, string version, string practiceid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters = new Dictionary<string, string>()
      {
        {"limit", "1"},
        { "offset", "0"},
        { "providerlist", "false" },
        { "showalldepartments", "true" }
      };

            // Now we get to make the URL, making sure to encode the parameters and remember the "?"
            string url = Utils.PathJoin(baseUrl, version, practiceid, "/departments", "?" + Utils.UrlEncode(parameters));

            // Create the request, add in the auth header
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + token;

            // Get the response, read and decode
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, Encoding.UTF8);
            Newtonsoft.Json.Linq.JObject departments = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd());
            Console.WriteLine(departments.ToString());

            response.Close();
        }

        static public void Main1()
        {
            // Set everything up
            string key = "cezsucv9zy2k3uu464qmkf4z";
            string secret = "bHZfSdakTnCNmVH";
            string version = "preview1";
            string practiceid = "195900";

            string baseUrl = "https://api.athenahealth.com/";

            // We use this a lot.
            Encoding UTF8 = System.Text.Encoding.GetEncoding("utf-8");

            // Easier to keep track of OAuth prefixes
            Dictionary<string, string> auth_prefixes = new Dictionary<string, string>()
      {
        {"v1", "/oauth"},
        {"preview1", "/oauthpreview"},
        {"openpreview1", "/oauthopenpreview"},
      };


            // Basic access authentication
            Dictionary<string, string> parameters = new Dictionary<string, string>()
      {
        {"grant_type", "client_credentials"},
      };

            // Create and set up a request
            WebRequest request = WebRequest.Create(Utils.PathJoin(baseUrl, auth_prefixes[version], "/token"));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // Make sure to add the Authorization header
            string auth = System.Convert.ToBase64String(UTF8.GetBytes(key + ":" + secret));
            request.Headers["Authorization"] = "Basic " + auth;

            // Encode the parameters, convert it to bytes (because that's how the streams want it)
            string encoded = Utils.UrlEncode(parameters);
            byte[] content = Encoding.UTF8.GetBytes(encoded);

            // Write the parameters to the body
            Stream writer = request.GetRequestStream();
            writer.Write(content, 0, content.Length);
            writer.Close();

            // Get the response, read it out, and decode it
            WebResponse response = request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader reader = new StreamReader(receive, UTF8);
            //JsonValue authorization = JsonValue.Parse(reader.ReadToEnd());
            //Object authorization = Newtonsoft.Json.JsonConvert.DeserializeObject(reader.ReadToEnd());
            Newtonsoft.Json.Linq.JObject authorization = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd());
            // Make sure to grab the token!
            string token = authorization["access_token"].ToString();
            Console.WriteLine(token);

            // And always remember to close the readers and streams
            response.Close();

            // GET /departments

            // Since GET parameters go in the URL, we set up the parameters Dictionary first
            parameters = new Dictionary<string, string>()
          {
            {"limit", "1"},
          };

            // Now we get to make the URL, making sure to encode the parameters and remember the "?"
            string url = Utils.PathJoin(baseUrl, version, practiceid, "/departments", "?" + Utils.UrlEncode(parameters));

            // Create the request, add in the auth header
            request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + token;

            // Get the response, read and decode
            response = request.GetResponse();
            receive = response.GetResponseStream();
            reader = new StreamReader(receive, UTF8);
            Newtonsoft.Json.Linq.JObject departments = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd());
            Console.WriteLine(departments.ToString());

            response.Close();


            // POST /appointments/{appointmentid}/notes

            url = Utils.PathJoin(baseUrl, version, practiceid, "/appointments/1/notes");
            parameters = new Dictionary<string, string>()
      {
        {"notetext", "Hello from C#"},
      };

            request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers["Authorization"] = "Bearer " + token;

            content = UTF8.GetBytes(Utils.UrlEncode(parameters));
            writer = request.GetRequestStream();
            writer.Write(content, 0, content.Length);
            writer.Close();

            response = request.GetResponse();
            receive = response.GetResponseStream();
            reader = new StreamReader(receive, UTF8);
            Newtonsoft.Json.Linq.JObject note = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd());
            Console.WriteLine(note.ToString());

            response.Close();
        }
    }
}