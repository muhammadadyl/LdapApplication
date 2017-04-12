using LdapApplication.Model;
using LdapApplication.Services;
using LdapApplication.Services.Interfaces;
using LdapApplication.Services.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LdapApplication.WebApi
{
    public partial class Startup
    {
        // The secret key every token will be signed with.
        // Keep this safe on the server!
        private static readonly string secretKey = "mysupersecret_secretkey!123";

        private void ConfigureAuth(IApplicationBuilder app)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            app.UseSimpleTokenProvider(new TokenProviderOptions
            {
                Path = "/api/token",
                Audience = "ssouser",
                Issuer = "ssoidentityapp",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "ssoidentityapp",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "ssouser",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters,
                
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = false,
                AuthenticationScheme = "Cookie",
                CookieName = "access_token",
                TicketDataFormat = new CustomJwtDataFormat(
                    SecurityAlgorithms.HmacSha256,
                    tokenValidationParameters)
            });

        }

        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {

            IAuthenticationService _authentication = new LdapAuthenticationService(
                Options.Create<LdapConfig>(
                new LdapConfig {
                    Url = Configuration["ldap:url"],
                    Port = int.Parse(Configuration["ldap:port"] ?? "0"),
                    BindDn = Configuration["ldap:bindDn"],
                    BindCredentials = Configuration["ldap:bindCredentials"],
                    SearchBase = Configuration["ldap:searchBase"],
                    SearchFilter = Configuration["ldap:searchFilter"]
                }
            ));

            AppUser user = _authentication.Login(username, password);
            if(user != null)
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(user.Username, "Token"), new Claim[] { }));

            #region Test Stub Code

            //// Don't do this in production, obviously!
            //if (username == "TEST" && password == "TEST123")
            //{
            //    return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
            //} 
            #endregion

            // Credentials are invalid, or account doesn't exist
            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}