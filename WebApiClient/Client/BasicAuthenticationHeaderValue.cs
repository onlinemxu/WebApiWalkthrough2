using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient.Client
{
    public class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
    {
        public BasicAuthenticationHeaderValue(string userName, string password)
            : base("Basic", EncodeCredential(userName, password))
        {
        }

        private static string EncodeCredential(string userName, string password)
        {
            string credential = string.Format("{0}:{1}", userName, password);

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(credential));
        }
    }

    public class TokenAuthenticationHeaderValue : AuthenticationHeaderValue
    {
        public TokenAuthenticationHeaderValue(string accessToken) 
            : base("Bearer", accessToken)
        {
        }
    }
}
