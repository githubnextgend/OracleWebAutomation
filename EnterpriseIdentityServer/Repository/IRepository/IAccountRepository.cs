using EnterpriseIdentityServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseIdentityServer.Repository.IRepository
{
    public interface IAccountRepository
    {
        Account GetAccount(string username, string password);

        void InsertAccount(string username, string password,string oranization, string email, string phone, out Guid userGuid);
    }
}
