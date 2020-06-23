using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Demetrios.Models;
using System;
using System.Collections.Generic;

namespace Demetrios.Testes
{
    public class TestesRota
    {
        private readonly HttpClient _client;

        public TestesRota()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Theory]
        [InlineData("GET")]
        public async Task TesteRotaGetMinutoPostsAndCreate(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/Minutos/GetMinutoPostsAndCreate");

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET")]
        public async Task TesteRotaIncorretaGetMinutoPostsAndCreate(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/MinutosXXX/GetMinutoPostsAndCreate");

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("GET")]
        public async Task TestRotaMinutos(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/Minutos/GetMinutoPostsAndCreate");

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                var request2 = new HttpRequestMessage(new HttpMethod(method), "/Minutos");

                var response2 = await _client.GetAsync(request2.RequestUri);

                Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            }
        }

        [Theory]
        [InlineData("GET")]
        public async Task TestRetornaMinutos(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/Minutos/GetMinutoPostsAndCreate");

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                var request2 = new HttpRequestMessage(new HttpMethod(method), "/Minutos");

                var response2 = await _client.GetAsync(request2.RequestUri);

                if (response2.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var responseString = response2.Content.ReadAsStringAsync().Result.ToString();

                    var r = JsonConvert.DeserializeObject<List<MinutoPost>>(responseString);

                    Assert.Equal(10, r.Count);
                }
            }
        }
    }
}
