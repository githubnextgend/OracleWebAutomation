using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseIdentityServer.Authorities;
using EnterpriseIdentityServer.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EnterpriseIdentityServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private Dictionary<string, AuthorityIssuer> _issuers;

        public AuthorityController(ILogger<AuthorityController> logger, IAccountRepository authenticationRepository)
        {
            //For testing purpose
            authenticationRepository.InsertAccount("vee", "qwertyui","orgname","email@g.com", "+66821113334", out Guid guid);

            _issuers = new Dictionary<string, AuthorityIssuer>()
        {
            {
                "owner",
                AuthorityIssuer.Create(new AuthenticationAuthority(), "identity")
                    .Register("account", new AccountAuthority(authenticationRepository))
                    .Register("otp", new OTPAuthority(logger))
            }
        };
        }
    }
}