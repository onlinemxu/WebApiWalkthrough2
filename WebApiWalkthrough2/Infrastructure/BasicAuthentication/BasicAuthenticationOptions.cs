using System.IO.Ports;
using Microsoft.Owin.Security;

namespace WebApiWalkthrough2.Infrastructure.BasicAuthentication
{
    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public BasicAuthenticationOptions(string realm, BasicAuthenticationMiddleware.CredentialValidationFunction validationFunction)
            : base("Basic")
        {
            Realm = realm;
            CredentialValidation = validationFunction;
        }

        public string Realm { get; set; }

        public BasicAuthenticationMiddleware.CredentialValidationFunction CredentialValidation { get; set; }
    }
}