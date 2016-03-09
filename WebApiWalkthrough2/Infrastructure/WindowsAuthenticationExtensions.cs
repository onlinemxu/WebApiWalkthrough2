﻿using System.Net;
using Owin;

namespace WebApiWalkthrough2.Infrastructure
{
    public static class WindowsAuthenticationExtensions
    {
        public static IAppBuilder UseWindowsAuthentication(this IAppBuilder app)
        {
            object value;

            if (app.Properties.TryGetValue("System.Net.HttpListener", out value))
            {
                var listener = value as HttpListener;
                if (listener != null)
                {
                    listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;
                }
            }

            return app;
        }
    }
}