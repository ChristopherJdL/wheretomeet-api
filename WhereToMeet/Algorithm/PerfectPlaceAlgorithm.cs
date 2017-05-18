using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereToMeet.Services;
using WhereToMeet.Services.PlacesProviders;
using WhereToMeet.Transporters.Output;
using WhereToMeet.Transporters;
using WhereToMeet.Transporters.Input;

namespace WhereToMeet.Algorithm
{
    public class PerfectPlaceAlgorithm
    {
        public PerfectPlaceAlgorithm()
        {

        }
        public async Task<PlaceTransporter> FindPerfectPlace(IPlacesProvider placeProvider, String[] placesTypes,
            IDistanceResolver distanceResolver, GeoCoordinatesTransporter[] geoCoordinates)
        {
            double sumX = 0.0, sumY = 0.0;
            foreach (GeoCoordinatesTransporter item in geoCoordinates)
            {
                sumX += item.X;
                sumY += item.Y;
            }

            var averageCoordinates = new GeoCoordinatesTransporter();
            averageCoordinates.X = sumX / geoCoordinates.Length;
            averageCoordinates.Y = sumY / geoCoordinates.Length;

            var candidatePlaces = await placeProvider.LookForNearbyPlacesAsync(new PlacesQueryTransporter
                                                                                                   {
                                                                                                     Latitude = averageCoordinates.Y,
                                                                                                     Longitude = averageCoordinates.X,
                                                                                                     Radius = 500,
                                                                                                     PlacesTypes = placesTypes
                                                                                                   });
            int minimumMinute = 0;
            PlaceTransporter finalPlace = null;
            foreach (var item1 in candidatePlaces)
            {
                int minute = 0;
                foreach (GeoCoordinatesTransporter item2 in geoCoordinates)
                {
                    minute += (await distanceResolver.ResolveDuration(new GeoCoordinatesTransporter() { X = item1.Longitude, Y = item1.Latitude } , item2) / 60);
                }
                if (finalPlace == null || minimumMinute < minute)
                {
                    minimumMinute = minute;
                    finalPlace = item1;
                }
            }

            return finalPlace;
        }
    }
}
