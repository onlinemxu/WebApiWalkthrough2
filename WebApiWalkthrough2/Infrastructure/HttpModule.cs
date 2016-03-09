using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Web;

namespace WebApiWalkthrough2.Infrastructure
{
    public class HttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        public void Dispose()
        {
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            Helper.Write("HttpModule", HttpContext.Current.User);
        }
    }

    internal static class Helper
    {
        public static void Write(string stage, IPrincipal principal)
        {
            Debug.WriteLine(string.Format("----- {0} -----", stage));

            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                Debug.WriteLine("anonymous user");
            }
            else
            {
                Debug.WriteLine(string.Format("User: {0}", principal.Identity.Name));
            }

            Debug.WriteLine("\n");
        }
    }
}