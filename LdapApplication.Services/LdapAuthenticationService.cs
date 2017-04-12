using LdapApplication.Model;
using LdapApplication.Services.Interfaces;
using LdapApplication.Services.Models;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;

namespace LdapApplication.Services
{
    public class LdapAuthenticationService : IAuthenticationService
    {
        private const string Uid = "uid";
        private const string DisplayNameAttribute = "cn";
        private const string MailAttribute = "mail";

        private readonly LdapConfig _config;
        private readonly LdapConnection _connection;

        public LdapAuthenticationService(IOptions<LdapConfig> config)
        {
            _config = config.Value;
            _connection = new LdapConnection
            {
                SecureSocketLayer = false
            };
        }

        public AppUser Login(string username, string password)
        {
            _connection.Connect(_config.Url, _config?.Port == 0 ? LdapConnection.DEFAULT_SSL_PORT : _config.Port);
            _connection.Bind(_config.BindDn, _config.BindCredentials);

            var searchFilter = string.Format(_config.SearchFilter, username);
            var result = _connection.Search(
                _config.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { Uid, DisplayNameAttribute, MailAttribute },
                false
            );

            try
            {
                var user = result.next();
                if (user != null)
                {
                    _connection.Bind(user.DN, password);
                    if (_connection.Bound)
                    {
                        return new AppUser
                        {
                            DisplayName = user.getAttribute(DisplayNameAttribute).StringValue,
                            Username = user.getAttribute(Uid).StringValue,
                            Mail = user.getAttribute(MailAttribute).StringValue
                        };
                    }
                }
            }
            catch
            {
                throw new Exception("Login failed.");
            }
            _connection.Disconnect();
            return null;
        }
    }
}
