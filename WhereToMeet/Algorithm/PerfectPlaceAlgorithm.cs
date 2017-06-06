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

        //public GeoCoordinatesTransporter    ObtainAverageCoordinates(GeoCoordinatesTransporter[] geoCoordinates)
        //{
        //    double x = 0d;
        //    double y = 0d;
        //    double z = 0d;
        //    geoCoordinates.Take(2).ToList().ForEach(g => x = x + (Math.Cos(g.Y) * Math.Cos(g.X)));
        //    geoCoordinates.Take(2).ToList().ForEach(g => y = y + (Math.Cos(g.Y) * Math.Sin(g.X)));
        //    geoCoordinates.Take(2).ToList().ForEach(g => z = z + (Math.Sin(g.Y)));
        //    double N = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
        //    double latitudeM = Math.Asin(z / N);
        //    double longitudeM = 0;
        //    if (x >= 0)
        //        longitudeM = Math.Asin(y / N * Math.Cos(Math.Asin(z / N)));
        //    else
        //        longitudeM = Math.Sign(y) * (Math.PI - Math.Asin(Math.Abs(y) / N * Math.Cos(Math.Asin(z / N))));
        //    return new GeoCoordinatesTransporter()
        //    {
        //        Y = latitudeM,
        //        X = longitudeM
        //    };
        //}

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

        public static double CalcDistance(double Lat1,
     double Long1, double Lat2, double Long2)
        {
            /*
                The Haversine formula according to Dr. Math.
                http://mathforum.org/library/drmath/view/51879.html

                dlon = lon2 - lon1
                dlat = lat2 - lat1
                a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
                c = 2 * atan2(sqrt(a), sqrt(1-a)) 
                d = R * c

                Where
                    * dlon is the change in longitude
                    * dlat is the change in latitude
                    * c is the great circle distance in Radians.
                    * R is the radius of a spherical Earth.
                    * The locations of the two points in 
                        spherical coordinates (longitude and 
                        latitude) are lon1,lat1 and lon2, lat2.
            */
            double dDistance = Double.MinValue;
            double dLat1InRad = Lat1 * (Math.PI / 180.0);
            double dLong1InRad = Long1 * (Math.PI / 180.0);
            double dLat2InRad = Lat2 * (Math.PI / 180.0);
            double dLong2InRad = Long2 * (Math.PI / 180.0);

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
