using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiClient.Client;

namespace WebApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine("Enter to stop");
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


            //add a company
            var companyClient = new CompanyClient("http://localhost:58549/api2", authHeader);
            var companies = companyClient.GetCompanies();
            var nextId = companies.Max(c => c.Id) + 1;

            var result = companyClient.AddCompany(new Company
            {
                Name = $"New Company {nextId}"
            });

            WriteStatusCodeResult(result);
            Console.WriteLine(content);
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
    }
}
