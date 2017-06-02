using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WhereToMeet.Tests.Utils
{
    class TestAppHolder
    {
        internal const string authScheme = "Bearer";
        public HttpClient Client { get; }
        protected string Token { get; set; }
        protected AuthenticationTestsShould authTests;

        private static TestAppHolder instance;

        public static TestAppHolder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TestAppHolder();
                }
                return instance;
            }
        }
        protected TestAppHolder()
        {
            this.Token = null;
            this.authTests = new AuthenticationTestsShould();
            this.Client = authTests.Client;
        }
        protected async Task<string> GetTokenByRequest()
        {
            string returnedToken = await authTests.ReturnValidTokenForRequest();
            this.Token = returnedToken; //set the token
            return returnedToken;
        }
        public async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrEmpty(this.Token))
                return await this.GetTokenByRequest();
            else
                return this.Token;
        }
    }
}
