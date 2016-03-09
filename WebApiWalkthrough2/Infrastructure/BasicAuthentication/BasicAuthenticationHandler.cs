using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace WebApiWalkthrough2.Infrastructure.BasicAuthentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private const string OwinAuthBasic = "Basic";
        private readonly string _challenge;

        public BasicAuthenticationHandler(BasicAuthenticationOptions options)
        {
            _challenge = string.Format("Basic realm={0}", options.Realm);
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var authValue = Request.Headers.Get("Authorization");

            if (string.IsNullOrEmpty(authValue) || !authValue.StartsWith(OwinAuthBasic, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var token = authValue.Substring(OwinAuthBasic.Length + 1).Trim();
            var claims = await TryGetPrincipalFromBasicCredentials(token, Options.CredentialValidation);

            if (claims == null)
            {
                return null;
            }

            var id = new ClaimsIdentity(claims, Options.AuthenticationType);

            return new AuthenticationTicket(id, new AuthenticationProperties());

        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode == 401)
            {
                var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);
                if (challenge != null)
                {
                    Response.Headers.AppendValues("WWW-Authenticate", _challenge);
                }
            }

            return Task.FromResult<object>(null);
        }

        private async Task<IEnumerable<Claim>> TryGetPrincipalFromBasicCredentials(string credentials,
            BasicAuthenticationMiddleware.CredentialValidationFunction validateCredential)
        {
            string pair;

            try
            {
                pair = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
            }
            catch (FormatException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            var index = pair.IndexOf(':');
            if (index == -1)
            {
                return null;
            }

            var userName = pair.Substring(0, index);
            var password = pair.Substring(index + 1);

            return await validateCredential(userName, password);
        }
    }
}