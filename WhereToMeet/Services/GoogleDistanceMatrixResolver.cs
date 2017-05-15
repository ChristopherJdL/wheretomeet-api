using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WhereToMeet.Transporters;
using Newtonsoft.Json;
using WhereToMeet.Transporters;
using WhereToMeet.Transporters.Output.GoogleDistanceMatrix;
using WheretoMeet.Services;

namespace WhereToMeet.Services
{
    class GoogleDistanceMatrixResponseStatus
    {
        public const string Ok = "OK";
        public const string NotFound = "NOT_FOUND";
        public const string ZeroResults = "ZERO_RESULTS";
        public const string InvalidRequest = "INVALID_REQUEST";
    }
    public class GoogleDistanceMatrixResolver : IDistanceResolver
    {
        protected HttpClient Client { get; set; }
        protected IConfigurationRoot Configuration { get; }
        public GoogleDistanceMatrixResolver(IConfigurationRoot configuration)
        {
            this.Client = new HttpClient()
            {
                BaseAddress = new Uri(configuration["GeoDataServices:GoogleDistanceMatrix:Url"])
            };
            this.Configuration = configuration;
        }
        
        public int ObtainDurationFromMatrix(DistanceMatrixResponse gMatrixTransporter)
        {
            var firstRow = gMatrixTransporter.Rows.First();
            var firstElement = firstRow.Elements.First();
            if (firstElement.Status == GoogleDistanceMatrixResponseStatus.Ok)
                return firstElement.Duration.Value;
            return -1;
        }

        public async Task<int> ResolveDuration(GeoCoordinatesTransporter origin, GeoCoordinatesTransporter destination)
        {
            var parameters = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("origins", origin.ToString()),
                new KeyValuePair<string, string>("destinations", destination.ToString()),
                new KeyValuePair<string, string>("mode", Configuration["GeoDataServices:GoogleDistanceMatrix:TransportationMode"])
            });
            var response = await this.Client.GetAsync($"?{await parameters.ReadAsStringAsync()}");
            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                var gMatrixTransporter = JsonConvert.DeserializeObject<DistanceMatrixResponse>(responseString);
                if (gMatrixTransporter.Status == GoogleDistanceMatrixResponseStatus.Ok)
                    return ObtainDurationFromMatrix(gMatrixTransporter);
            }
            return -1;
        }

        public Task<int> ResolveDistance(GeoCoordinatesTransporter origin, GeoCoordinatesTransporter destination)
        {
            throw new NotImplementedException();
        }
    }
}
