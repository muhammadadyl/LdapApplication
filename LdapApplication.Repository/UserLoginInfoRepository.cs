using LdapApplicaiton.Data;
using LdapApplication.Model;
using LdapApplication.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System;

namespace LdapApplication.Repository
{
    public class UserLoginInfoRepository : Repository<UserLoginInfo>, IRepository<UserLoginInfo>
    {
        public UserLoginInfoRepository(DbContext db) : base(db)
        {
        }
    }
}
