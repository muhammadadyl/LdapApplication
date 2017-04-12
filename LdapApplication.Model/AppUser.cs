using System;
using System.ComponentModel.DataAnnotations;

namespace LdapApplication.Model
{
    public class AppUser
    {
        [Key]
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Mail { get; set; }
    }
}
