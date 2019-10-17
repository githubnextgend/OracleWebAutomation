using OCIClientLibCore.Helpers.OCISignerHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace OCIClientLib.Helpers
{
    public class OCIClientHelper
    {
        public string tenancyId { get; set; }
        public string compartmentId { get; set; }
        public string userId { get; set; }
        public string fingerPrint { get; set; }
        public string privateKeyPath { get; set; }
        public string privateKeyPassphrase { get; set; }
        public string region { get; set; }
        public string timezone { get; set; }
        public string instanceStartHrDaily { get; set; }
        public string instanceStopHrDaily { get; set; }

        // Oracle Cloud Infrastructure APIs require TLS 1.2
        // uncomment the line below if targeting < .NET Framework 4.6 (HttpWebRequest does not enable TLS 1.2 by default in earlier versions)
        // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


        public string ExecuteRequest(HttpWebRequest request)
        {
            try
            {
                var webResponse = (HttpWebResponse)request.GetResponse();
                var response = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
                // Console.WriteLine($"Response: {response}");

                return response;
            }
            catch (WebException e)
            {
                Console.WriteLine($"Exception occurred: {e.Message}");
                //Console.WriteLine($"Response: {new StreamReader(e.Response.GetResponseStream()).ReadToEnd()}");

                return String.Empty;
            }
        }


        public string GetUsers()
        {
            var uri = new Uri("https://identity." + this.region + ".oraclecloud.com/20160918/users/" + this.userId);

            // var uri = new Uri(url);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.Accept = "application/json";

            var signer = new RequestSigner(tenancyId, userId, fingerPrint, privateKeyPath, privateKeyPassphrase);
            signer.SignRequest(request);

            var response = ExecuteRequest(request);
            Console.WriteLine(response);
            return response;
        }

        //public string GetInstances()
        //{
        //    //GET /20160918/instances/?availabilityDomain=MsFN%3AAP-SYDNEY-1-AD-1&compartmentId=ocid1.compartment.oc1..aaaaaaaaa2sevxdnsfngp5jxzl7ej5oqdnq77yad3maramyp3xz2vnyrxsya&sortBy=timeCreated&sortOrder=DESC HTTP/1.1
        //    //var uri = new Uri($"https://identity.us-ashburn-1.oraclecloud.com/20160918/users/{userId}");

        //    //url=https://iaas.ap-sydney-1.oraclecloud.com/20160918/instances/?availabilityDomain=MsFN%3AAP-SYDNEY-1-AD-1&compartmentId=ocid1.compartment.oc1..aaaaaaaaa2sevxdnsfngp5jxzl7ej5oqdnq77yad3maramyp3xz2vnyrxsya&sortBy=timeCreated&sortOrder=DESC

        //    //var uri = new Uri(string.Format(@"https://identity.us-ashburn-1.oraclecloud.com/20160918/users/" + this.userId));

        //    var uri = new Uri("https://iaas.ap-sydney-1.oraclecloud.com/20160918/instances?compartmentId="+this.compartmentId);
        //    // var uri = new Uri(url);
        //    var request = (HttpWebRequest)WebRequest.Create(uri);
        //    request.Method = "GET";
        //    request.Accept = "application/json";

        //    var signer = new RequestSigner(tenancyId, userId, fingerPrint, privateKeyPath, privateKeyPassphrase);
        //    signer.SignRequest(request);

        //    Console.WriteLine($"Authorization header: {request.Headers["authorization"]}");
        //    Console.WriteLine("=========================================================");
        //    Console.WriteLine("REQUEST");
        //    Console.WriteLine(request.Address.ToString());
        //    Console.WriteLine(request.Headers.ToString());
        //    Console.WriteLine("=========================================================");
        //    Console.WriteLine("RESPONSE");
        //    var response = ExecuteRequest(request);
        //    Console.WriteLine(request);
        //    return response;
        //}

        public string StartInstancePool(string instanceid, string mode)//mode=start/stop
        {
            // ​POST / 20160918 / instancePools /{ instancePoolId}/ actions / start
            var uri = new Uri("https://iaas." + this.region + ".oraclecloud.com/20160918/instances/" + instanceid + "/actions/" + mode);
            var body = "";// string.Format(@"{{""cidrblock"" : ""10.0.0.0/16"",""compartmentid"" : ""{0}"",""displayname"" : ""myvcn""}}", this.compartmentId);
            var bytes = Encoding.UTF8.GetBytes(body);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            //request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers["x-content-sha256"] = Convert.ToBase64String(SHA256.Create().ComputeHash(bytes));

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            var signer = new RequestSigner(tenancyId, userId, fingerPrint, privateKeyPath, privateKeyPassphrase);
            signer.SignRequest(request);
            //Console.WriteLine($"authorization header: {request.Headers["authorization"]}");
            //Console.WriteLine("=========================================================");
            //Console.WriteLine("REQUEST");
            //Console.WriteLine(request.Address.ToString());
            //Console.WriteLine(request.Headers.ToString());
            //Console.WriteLine("=========================================================");
            //Console.WriteLine("RESPONSE");
            var response = ExecuteRequest(request);
            Console.WriteLine(response);
            return response;

        }

        public string CreateVCN(RequestSigner signer, string url = "https://iaas.ap-sydney-1.oraclecloud.com/20160918/vcns")
        {
            try
            {
                // POST with body (creates a VCN)
                var uri = new Uri($"https://iaas.ap-sydney-1.oraclecloud.com/20160918/vcns");
                var body = string.Format(@"{{""cidrBlock"" : ""10.0.0.0/16"",""compartmentId"" : ""{0}"",""displayName"" : ""mytest""}}", compartmentId);
                var bytes = Encoding.UTF8.GetBytes(body);

                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers["x-content-sha256"] = Convert.ToBase64String(SHA256.Create().ComputeHash(bytes));

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                signer.SignRequest(request);
                var response = ExecuteRequest(request);
                return response;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }





    }
}
