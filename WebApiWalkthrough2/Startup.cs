using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
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
           
            //app.UseWindowsAuthentication();
            //app.UseClaimsTransformation(Transformation);
            app.UseBasicAuthentication("demo", ValidateUsers);
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        private Task<IEnumerable<Claim>> ValidateUsers(string id, string secret)
        {
            if (id == secret)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Role, "Basic Role")
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