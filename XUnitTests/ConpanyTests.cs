using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using WebApiTest;
using Microsoft.Extensions.Configuration;

namespace WebApiTests.Tests
{
    public class ConpanyTests
    {
        private readonly HttpClient _client;

        public ConpanyTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.text.json")
                    .Build()
                )
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Theory]
        [InlineData("GET")]
        public async Task TestGetCompanies(string method)
        {
            // arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "api/companies/google");
            // act
            var response = await _client.SendAsync(request);
            //assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET")]
        public async Task TestGetCompany(string method)
        {
            // arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "api/companies/bynumber/09589217");
            // act
            var response = await _client.SendAsync(request);
            //assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET")]
        public async Task TestGetCompanyOfficers(string method)
        {
            // arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "api/companies/bynumber/09589217/officers");
            // act
            var response = await _client.SendAsync(request);
            //assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
