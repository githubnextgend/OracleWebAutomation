using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseIdentityServer.Model
{
    public class EnterpriseIdentityDbContext: DbContext
    {
        public EnterpriseIdentityDbContext() { }
        public EnterpriseIdentityDbContext(DbContextOptions<EnterpriseIdentityDbContext> options) : base(options) { }
        public virtual DbSet<Account> Accounts { get; set; }
    }
}
