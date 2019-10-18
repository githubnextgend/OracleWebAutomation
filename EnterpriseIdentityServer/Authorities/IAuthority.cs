using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EnterpriseIdentityServer.Authorities
{
    public interface IAuthority
    {
        string[] Payload { get; }
        Claim[] OnVerify(Claim[] claims, JObject payload, string identifier, out bool valid);
        Claim[] OnForward(Claim[] claims);
    }


    public interface IAuthenticator
    {
        Claim[] GetAuthenticationClaims(string identifier);
    }
}
