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
using System.Globalization;

namespace WhereToMeet.Algorithm
{
    public class PerfectPlaceAlgorithm
    {
        public PerfectPlaceAlgorithm()
        {

        }

        public async Task<PlaceTransporter> DefaultBehaviour(GeoCoordinatesTransporter[] geoCoordinates, IPlacesProvider placesProvider, IEnumerable<string> placesTypes, int radius)
        {
            var foundPlaces = await placesProvider.LookForNearbyPlacesAsync(new PlacesQueryTransporter()
            {
                Latitude = geoCoordinates.First().Y,
                Longitude = geoCoordinates.First().X,
                PlacesTypes = placesTypes,
                Radius = radius
            });
            return foundPlaces.Last();
        }

        public GeoCoordinatesTransporter GetAverageLocation(GeoCoordinatesTransporter[] geoCoordinates)
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
            return averageCoordinates;
        }

        /// <summary>
        /// The Haversine formula according to Dr. Math.  
        ///  
        /// http://mathforum.org/library/drmath/view/51879.html
        ///
        /// </summary>
        /// <param name="LatLhs">Left hand spherical latitude coordinate.</param>
        /// <param name="LongLhs">Left hand spherical longitude coordinate.</param>
        /// <param name="LatRhs">Right hand spherical latitude coordinate.</param>
        /// <param name="LongRhs">Right hand spherical longitude coordinate.</param>
        /// <returns></returns>
        public static double CalcDistance(double LatLhs,
     double LongLhs, double LatRhs, double LongRhs)
        {
            double dDistance = Double.MinValue;
            double dLat1InRad = LatLhs * (Math.PI / 180.0);
            double dLong1InRad = LongLhs * (Math.PI / 180.0);
            double dLat2InRad = LatRhs * (Math.PI / 180.0);
            double dLong2InRad = LongRhs * (Math.PI / 180.0);

            double dLongitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            // Distance.
            // const Double kEarthRadiusMiles = 3956.0;
            const Double kEarthRadiusKms = 6376.5;
            dDistance = kEarthRadiusKms * c;

            return dDistance;
        }

        public async Task<PlaceTransporter> FindPerfectPlace(IPlacesProvider placesProvider, String[] placesTypes,
            IDistanceResolver distanceResolver, GeoCoordinatesTransporter[] geoCoordinates, GeoCoordinatesTransporter leaderCoordinates)
        {
            var averageCoordinates = this.GetAverageLocation(geoCoordinates);
            var candidatePlaces = await placesProvider.LookForNearbyPlacesAsync(new PlacesQueryTransporter
                                                                                                   {
                                                                                                     Latitude = averageCoordinates.Y,
                                                                                                     Longitude = averageCoordinates.X,
                                                                                                     Radius = 1000,
                                                                                                     PlacesTypes = placesTypes
                                                                                                   });
            if (candidatePlaces.Count() == 0)
                return null;
            double minimumDistance = 0;
            PlaceTransporter finalPlace = null;
            foreach (var item1 in candidatePlaces)
            {
                double distance = 0;
                distance += CalcDistance(item1.Latitude, item1.Longitude, double.Parse(averageCoordinates.Y.ToString("N4", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture),
                             double.Parse(averageCoordinates.X.ToString("N4", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
                if (finalPlace == null || minimumDistance > distance)
                {
                    minimumDistance = distance;
                    finalPlace = item1;
                }
            }


            return finalPlace;
        }
    }
}
