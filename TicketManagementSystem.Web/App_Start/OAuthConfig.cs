using System;
using System.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Owin;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Web.Data;

namespace TicketManagementSystem.Web.App_Start
{
    public static class OAuthConfig
    {
        public static void ConfigureOAuth(this IAppBuilder app)
        {
            var oauthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(GetTokenLifeTime()),
                Provider = new CustomOAuthProvider(NinjectWebCommon.GetInstance().Get<IUserService>(),
                                                    NinjectWebCommon.GetInstance().Get<ILoginService>())
            };

            app.UseOAuthAuthorizationServer(oauthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private static int GetTokenLifeTime()
        {
            int lifeTime;

            if (!int.TryParse(ConfigurationManager.AppSettings["TokenLifeTimeMinutes"], out lifeTime))
            {
                lifeTime = 1440;
            }
            return lifeTime;
        }
    }
}