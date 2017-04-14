using LdapApplication.Model;
using LdapApplication.Services;
using LdapApplication.Services.Common;
using LdapApplication.Services.Interfaces;
using LdapApplication.Services.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
                IdentityResolver = GetIdentity,
                SaveLoginInfo = SaveLoginInfo
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

        private Task<UserLoginInfo> GetIdentity(HttpContext context, string username, string password)
        {
            IUserLoginInfoService userloginService = (IUserLoginInfoService)context.RequestServices.GetService(typeof(IUserLoginInfoService));
            UserLoginInfo user = userloginService.GetUserInfoByUsername(username);

            IAuthenticationService _authentication = (IAuthenticationService)context.RequestServices.GetService(typeof(IAuthenticationService));

            if (user != null)
                return Task.FromResult(user);

            user = _authentication.Login(username, password);
            if (user != null)
                return Task.FromResult(user);

            #region Test Stub Code

            //// Don't do this in production, obviously!
            //if (username == "TEST" && password == "TEST123")
            //{
            //    return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
            //} 
            #endregion

            // Credentials are invalid, or account doesn't exist
            return Task.FromResult<UserLoginInfo>(null);
        }

        private void SaveLoginInfo(HttpContext context, UserLoginInfo obj)
        {
            try
            {
                IService<UserLoginInfo> userloginService = (IService<UserLoginInfo>)context.RequestServices.GetService(typeof(IService<UserLoginInfo>));
                userloginService.Add(obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}