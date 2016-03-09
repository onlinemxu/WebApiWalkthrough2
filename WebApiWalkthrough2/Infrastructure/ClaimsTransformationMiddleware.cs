using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace WebApiWalkthrough2.Infrastructure
{
    public class ClaimsTransformationMiddleware
    {
        private readonly ClaimsTransformationOptions _options;
        private readonly Func<IDictionary<string, object>, Task> _next;

        public ClaimsTransformationMiddleware(Func<IDictionary<string, object>, Task> next, ClaimsTransformationOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);

            if (context.Authentication != null &&
                context.Authentication.User != null &&
                context.Authentication.User.Identity != null)
            {
                var transformedPrincipal = await _options.ClaimsTransformation(context.Authentication.User);
                context.Authentication.User = new ClaimsPrincipal(transformedPrincipal);
            }

            await _next(env);
        }
    }
}