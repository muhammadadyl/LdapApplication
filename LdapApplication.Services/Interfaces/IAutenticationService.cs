using LdapApplication.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LdapApplication.Services.Interfaces
{
    public interface IAuthenticationService
    {
        AppUser Login(string username, string password);
    }
}
