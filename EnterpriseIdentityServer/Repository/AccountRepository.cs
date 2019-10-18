using EnterpriseIdentityServer.Model;
using EnterpriseIdentityServer.Repository.IRepository;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseIdentityServer.Repository
{
    public class AccountRepository:IAccountRepository
    {
        private EnterpriseIdentityDbContext _db;
        public AccountRepository(EnterpriseIdentityDbContext context)
        {
            _db = context;
        }

        public Account GetAccount(string username, string password)
        {
            return _db.Accounts.SingleOrDefault(m => m.Username == username && m.EncryptedPassword == password.Sha256());
        }

        public void InsertAccount(string username, string password,string organization, string email, string phone, out Guid userGuid)
        {
            userGuid = Guid.NewGuid();
            _db.Accounts.Add(new Account()
            {
                UserGuid = userGuid,
                Username = username,
                EncryptedPassword = password.Sha256(),
                Organization=organization,
                Email=email,
                Phone = phone
            });
        }

    }
}
