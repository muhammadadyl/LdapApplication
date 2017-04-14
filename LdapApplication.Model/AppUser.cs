using LdapApplication.Model.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace LdapApplication.Model
{
    public class AppUser : BaseEntity
    {
        [Key]
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Mail { get; set; }
    }
}
