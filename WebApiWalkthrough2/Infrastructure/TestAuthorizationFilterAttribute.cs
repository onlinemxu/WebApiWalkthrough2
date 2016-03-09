using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiWalkthrough2.Infrastructure
{
    public class TestAuthorizationFilterAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            Helper.Write("Test Authorization Filter", actionContext.RequestContext.Principal);

            return true;
        }
    }
}