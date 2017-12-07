using System;
using System.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Owin;
using TicketManagementSystem.Business.AppSettings;
using TicketManagementSystem.Business.Interfaces;

namespace TicketManagementSystem.Web.App_Start
{
    public static class OAuthConfig
    {
        public static void ConfigureOAuth(this IAppBuilder app)
        {
            var ninject = NinjectWebCommon.GetInstance();

            var oauthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(GetTokenLifeTime()),
                Provider = new CustomOAuthProvider(ninject.Get<IUserService>(),
                                                    ninject.Get<ILoginService>(),
                                                    ninject.Get<IAppSettingsService>())
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