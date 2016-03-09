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

            client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue("client user", "client user");

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(content);
        }
    }
}
