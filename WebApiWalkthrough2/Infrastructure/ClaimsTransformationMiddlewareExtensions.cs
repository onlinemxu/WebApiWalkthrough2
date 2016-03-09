using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Owin;

namespace WebApiWalkthrough2.Infrastructure
{
    public static class ClaimsTransformationMiddlewareExtensions
    {
        public static IAppBuilder UseClaimsTransformation(this IAppBuilder app, Func<ClaimsPrincipal, Task<ClaimsPrincipal>> transformation)
        {
            return app.UseClaimsTransformation(new ClaimsTransformationOptions
            {
                ClaimsTransformation = transformation
            });
        }

        public static IAppBuilder UseClaimsTransformation(this IAppBuilder app, ClaimsTransformationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.Use(typeof (ClaimsTransformationMiddleware), options);

            return app;
        }
    }
}