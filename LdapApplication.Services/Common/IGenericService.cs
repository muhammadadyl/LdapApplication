using LdapApplication.Repository.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LdapApplication.Services.Common
{
    public interface IGenericService
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
