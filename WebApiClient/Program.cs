using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApiClient.Client;

namespace WebApiClient
{
    class Program
    {
        private const string HostApi2Uri = "http://localhost:58549/api2";

        static void Main(string[] args)
        {
            try
            {
                RunRequest().Wait();
                //SendRequest();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerExceptions[0].Message);
                Console.WriteLine(ex.InnerExceptions[0].StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine("Press the Enter key to exit...");
            Console.ReadLine();
        }

        private static void SendRequest()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:58549/api2/identity")
            };

            //var credArray = Encoding.ASCII.GetBytes("clientuser:clientuser");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(credArray));

            var authHeader = new BasicAuthenticationHeaderValue("client user", "client user");
            client.DefaultRequestHeaders.Authorization = authHeader;

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);


            //add a company
            var companyClient = new CompanyClient(HostApi2Uri, authHeader);
            var companies = companyClient.GetCompanies();
            var nextId = companies.Max(c => c.Id) + 1;

            var result = companyClient.AddCompany(new Company
            {
                Name = $"New Company {nextId}"
            });

            WriteStatusCodeResult(result);
        }

        private static async Task RunRequest()
        {
            string _accessToken;
            Dictionary<string, string> _tokenDictionary;

            var provider = new ApiClientProvider(HostApi2Uri);

            _tokenDictionary = await provider.GetTokenDictionary("michael@temenos.com", "password");
            _accessToken = _tokenDictionary["access_token"];

            foreach (var token in _tokenDictionary)
            {
                Console.WriteLine($"Token: {token.Key} | {token.Value}");
            }

            var tokenAuthHeader = new TokenAuthenticationHeaderValue(_accessToken);
            var companyClient = new CompanyClient(HostApi2Uri, tokenAuthHeader);

            var companies = await companyClient.GetCompaniesAsync();
            WriteCompanyList(companies);

        }

        static void WriteStatusCodeResult(System.Net.HttpStatusCode statusCode)
        {
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Opreation Succeeded - status code {0}", statusCode);
            }
            else
            {
                Console.WriteLine("Opreation Failed - status code {0}", statusCode);
            }
            Console.WriteLine("");
        }

        private static void WriteCompanyList(IEnumerable<Company> companies)
        {
            foreach (var company in companies)
            {
                Console.WriteLine($"Id: {company.Id}, Name: {company.Name}");
            }
            Console.WriteLine("----- Read Companies Completed Successfully! ----");
            Console.WriteLine("");
        }
    }
}
