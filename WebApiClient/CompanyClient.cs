using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApiClient
{
    public class CompanyClient
    {
        private readonly string _hostUri;
        private readonly AuthenticationHeaderValue _authenticationHeader;

        public CompanyClient(string hostUri, AuthenticationHeaderValue authenticationHeader)
        {
            _hostUri = hostUri;
            _authenticationHeader = authenticationHeader;
        }

        private HttpClient _client;
        public HttpClient Client
        {
            get {
                if (_client == null)
                {
                    _client = CreateClient();
                }
                return _client;
            }
        }

        public HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(new Uri(_hostUri), "api/company");
            client.DefaultRequestHeaders.Authorization = _authenticationHeader;
            return client;
        }

        public IEnumerable<Company> GetCompanies()
        {
            HttpResponseMessage response;
            using (var client = CreateClient())
            {
                response = client.GetAsync(client.BaseAddress).Result;
            }
            var result = response.Content.ReadAsAsync<IEnumerable<Company>>().Result;
            return result;
        }

        public HttpStatusCode AddCompany(Company company)
        {
            HttpResponseMessage response;

            using (var client = CreateClient())
            {
                response = client.PostAsJsonAsync(client.BaseAddress, company).Result;
            }

            return response.StatusCode;
        }
    }

    [Serializable]
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}