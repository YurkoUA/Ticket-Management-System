using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using TicketManagementSystem.Business.AppSettings;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly IAppSettingsService _appSettingsService;

        public CustomOAuthProvider(IUserService userService, ILoginService loginService, IAppSettingsService appSettingsService)
        {
            _userService = userService;
            _loginService = loginService;
            _appSettingsService = appSettingsService;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var user = await _userService.FindByLoginDataAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect");
                context.Rejected();
                return;
            }

            var authTicket = new AuthenticationTicket(SetClaimsIdentity(context, user), new AuthenticationProperties());
            context.Validated(authTicket);

            _loginService.AddLogin(GetLoginData(context, user), _appSettingsService.RemoveOldLogins);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, UserDTO user)
        {
            var claims = new ClaimsIdentity("JWT", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer));
            claims.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName, ClaimValueTypes.String));
            claims.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role, ClaimValueTypes.String));

            claims.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "OWIN Provider", ClaimValueTypes.String));

            return claims;
        }

        private static LoginDTO GetLoginData(OAuthGrantResourceOwnerCredentialsContext context, UserDTO user)
        {
            return new LoginDTO
            {
                UserId = user.Id,
                Date = DateTime.UtcNow,
                Type = "JWT",
                IpAddress = context.Request.RemoteIpAddress,
                UserAgent = context.Request.Headers["User-Agent"],
                Host = $"{context.Request.Uri.Scheme}://{context.Request.Uri.Authority}"
            };
        }
    }
}
