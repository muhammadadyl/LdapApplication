using LdapApplication.Model;
using LdapApplication.Repository.Common;
using LdapApplication.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LdapApplication.Services
{

    public class UserLoginInfoService : Service<UserLoginInfo>, IUserLoginInfoService
    {
        public UserLoginInfoService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public UserLoginInfo GetUserInfoByUsername(string username)
        {
            return _repository.FindBy(a => a.Username == username).FirstOrDefault();
        }
    }
}
