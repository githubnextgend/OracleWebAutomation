using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseIdentityServer.Model
{
    public class AuthorityModel
    {
        public JObject payload { get; set; }
        public string token { get; set; }
    }
}
