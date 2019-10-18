using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseIdentityServer.Model
{
    public class Account
    {
        [Key]
        public Guid UserGuid { get; set; }

        public string Username { get; set; }
        public string EncryptedPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string Organization { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }
    }
}
