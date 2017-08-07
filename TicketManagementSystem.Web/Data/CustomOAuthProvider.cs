using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.Data
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private IUserService _userService;
        private ILoginService _loginService;

        public CustomOAuthProvider(IUserService userService, ILoginService loginService)
        {
            _userService = userService;
            _loginService = loginService;
        }

        public async override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var user = await _userService.FindByLoginDataAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect");
                context.Rejected();
            }

            var authTicket = new AuthenticationTicket(SetClaimsIdentity(context, user), new AuthenticationProperties());
            context.Validated(authTicket);

            _loginService.AddLogin(GetLoginData(context, user), RemoveOldLogins());
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        private ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, UserDTO user)
        {
            var claims = new ClaimsIdentity("JWT", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer));
            claims.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName, ClaimValueTypes.String));
            claims.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role, ClaimValueTypes.String));

            claims.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "OWIN Provider", ClaimValueTypes.String));

            return claims;
        }

        private LoginDTO GetLoginData(OAuthGrantResourceOwnerCredentialsContext context, UserDTO user)
        {
            return new LoginDTO
            {
                UserId = user.Id,
                Date = DateTime.UtcNow,
                Type = "JWT",
                IpAddress = context.Request.RemoteIpAddress,
                UserAgent = context.Request.Headers["User-Agent"]
            };
        }

        private bool RemoveOldLogins()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["RemoveOldLogins"]);
        }
    }
}
