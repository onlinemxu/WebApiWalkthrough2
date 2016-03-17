using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApiClient
{
    public class ApiClientProvider
    {
        string _hostUri;
        public string AccessToken { get; private set; }

        public ApiClientProvider(string hostUri)
        {
            _hostUri = hostUri;
        }

        public async Task<Dictionary<string, string>> GetTokenDictionary(string userName, string password)
        {
            HttpResponseMessage response;
            var pairs = new List<KeyValuePair<string,string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };

            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var tokenEndpoint = new Uri(new Uri(_hostUri), "Token");
                response = await client.PostAsync(tokenEndpoint, content);
            }

            var responseContext = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {responseContext}");
            }

            return GetTokenDictionary(responseContext);
        }

        private Dictionary<string, string> GetTokenDictionary(string responseContent)
        {
            var tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
            return tokenDictionary;
        }
    }
}
