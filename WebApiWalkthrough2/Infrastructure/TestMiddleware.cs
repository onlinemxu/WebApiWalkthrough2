using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace WebApiWalkthrough2.Infrastructure
{
    public class TestMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public TestMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);

            // authentication
            //context.Request.User = new GenericPrincipal(new GenericIdentity("demo user"), new string[] {"admin"});

            Helper.Write("Test Middleware", context.Request.User);

            await _next(env);
        }
    }
}