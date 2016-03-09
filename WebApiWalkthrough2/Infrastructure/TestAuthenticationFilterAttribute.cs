using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace WebApiWalkthrough2.Infrastructure
{
    public class TestAuthenticationFilterAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            Helper.Write("Test Authentication Filter", context.ActionContext.RequestContext.Principal);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
        }
    }
}