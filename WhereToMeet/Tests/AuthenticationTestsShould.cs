using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WhereToMeet.Transporters;

namespace WhereToMeet.Tests
{
    public class AuthenticationTestsShould
    {
        internal readonly string loginRoute = "/api/login";
        internal readonly string signInRoute = "/api/signIn";
        internal readonly string testUsername = "SunHee";
        internal readonly string testUserPassword = "caucau";

        protected TestServer _server;
        public HttpClient Client { get; set; }

        public AuthenticationTestsShould()
        {
            //Arrange
            this._server = new TestServer(new WebHostBuilder().UseStartup<StartupForTests>());
            this.Client = _server.CreateClient();
        }

        
        [Fact]
        public async Task<string> ReturnValidTokenForRequest()
        {
            //Arrange
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", this.testUsername),
                new KeyValuePair<string, string>("password", this.testUserPassword)
            });
            //Act
            var response = await Client.PostAsync(this.loginRoute, requestContent);
            //Assert
            response.EnsureSuccessStatusCode();
            //Act
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var loginOutputTransporter = JsonConvert.DeserializeObject<LogInOutputTransporter>(jsonResponse);
            //Assert
            Assert.NotNull(loginOutputTransporter.AppToken);
            Assert.NotEmpty(loginOutputTransporter.AppToken);
            return loginOutputTransporter.AppToken;
        }
    }
}
