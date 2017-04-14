using System;
using System.Collections.Generic;
using System.Text;

namespace LdapApplication.Model
{
    public class UserLoginInfo : AppUser
    {
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; }
    }
}
