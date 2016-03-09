using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebApiWalkthrough2.Models;

namespace WebApiWalkthrough2.Controllers
{
    [Authorize]
    public class IdentityController : ApiController
    {
        public IEnumerable<ViewClaim> Get()
        {
            var principal = User as ClaimsPrincipal;

            if (principal != null)
            {
                return from c in principal.Claims
                       select new ViewClaim
                       {
                           Type = c.Type,
                           Value = c.Value
                       };
            }

            return new ViewClaim[] {};
        }
    }
}