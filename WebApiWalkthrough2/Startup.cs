using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebApiWalkthrough2.Infrastructure;
using WebApiWalkthrough2.Infrastructure.BasicAuthentication;

namespace WebApiWalkthrough2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            app.CreatePerOwinContext(WalkthroughDbContext.Create);
            app.Use(typeof(TestMiddleware));

            ConfigureAuth(app);
           
            //app.UseWindowsAuthentication();
            //app.UseClaimsTransformation(Transformation);
            app.UseBasicAuthentication("demo", ValidateUsers);
            
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            var options = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

                AllowInsecureHttp = true
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private Task<IEnumerable<Claim>> ValidateUsers(string id, string secret)
        {
            if (!string.IsNullOrEmpty(id) && id == secret)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Name, id),
                    new Claim(ClaimTypes.Role, "Basic Role"),
                    new Claim(ClaimTypes.AuthenticationMethod, "Basic Authentication")
                };

                return Task.FromResult<IEnumerable<Claim>>(claims);
            }

            return Task.FromResult<IEnumerable<Claim>>(null);
        }

        private async Task<ClaimsPrincipal> Transformation(ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return incomingPrincipal;
            }

            var name = incomingPrincipal.Identity.Name;
            // look for name
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, name),
                new Claim(ClaimTypes.Role, "demo role"),
                new Claim(ClaimTypes.Email, "admin@temenos.com")
            };

            var id = new ClaimsIdentity("Windows");
            id.AddClaims(claims);

            return new ClaimsPrincipal(id);
        }
    }
}