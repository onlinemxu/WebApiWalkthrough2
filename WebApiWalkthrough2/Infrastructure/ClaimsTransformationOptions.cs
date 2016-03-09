using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApiWalkthrough2.Infrastructure
{
    public class ClaimsTransformationOptions
    {
        public Func<ClaimsPrincipal, Task<ClaimsPrincipal>> ClaimsTransformation { get; set; }
    }
}