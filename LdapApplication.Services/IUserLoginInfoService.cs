using LdapApplication.Model;

namespace LdapApplication.Services
{
    public interface IUserLoginInfoService
    {
        UserLoginInfo GetUserInfoByUsername(string username);
    }
}