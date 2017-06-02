﻿using System;
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
            double sumX = 0.0, sumY = 0.0, sumZ = 0.0;
            foreach (GeoCoordinatesTransporter item in geoCoordinates)
            {
                double x = Math.Sin(item.X * Math.PI / 180) * Math.Cos(item.Y * Math.PI / 180);
                double y = Math.Sin(item.X * Math.PI / 180) * Math.Sin(item.Y * Math.PI / 180);
                double z = Math.Cos(item.X * Math.PI / 180);
                sumX += x;
                sumY += y;
                sumZ += z;
            }
            double averageX = sumX / geoCoordinates.Length;
            double averageY = sumY / geoCoordinates.Length;
            double averageZ = sumZ / geoCoordinates.Length;
            double r = Math.Sqrt(averageX * averageX + averageY * averageY + averageZ * averageZ);

            var averageCoordinates = new GeoCoordinatesTransporter();
            averageCoordinates.X = Math.Acos(averageZ / r) * 180 / Math.PI;
            averageCoordinates.Y = Math.Acos(averageX / averageY) * 180 / Math.PI;
            return averageCoordinates;
        }

        public async Task<PlaceTransporter> FindPerfectPlace(IPlacesProvider placesProvider, String[] placesTypes,
            IDistanceResolver distanceResolver, GeoCoordinatesTransporter[] geoCoordinates, GeoCoordinatesTransporter leaderCoordinates)
        {
            var averageCoordinates = this.GetAverageLocation(geoCoordinates);
            var candidatePlaces = await placesProvider.LookForNearbyPlacesAsync(new PlacesQueryTransporter
                                                                                                   {
                                                                                                     Latitude = leaderCoordinates.Y,
                                                                                                     Longitude = leaderCoordinates.X,
                                                                                                     Radius = 10000,
                                                                                                     PlacesTypes = placesTypes
                                                                                                   });
            if (candidatePlaces.Count() == 0)
                return null;
            PlaceTransporter finalPlace = null;
            var initialDifference = new GeoCoordinatesTransporter() { Y = candidatePlaces.ElementAt(0).Latitude - averageCoordinates.Y, X = candidatePlaces.ElementAt(0).Longitude - averageCoordinates.X };
            
            for (int i = 0; i < candidatePlaces.Count(); i++)
            {
                var candidatePlace = candidatePlaces.ElementAt(i);
                double differenceLatitude = Math.Abs(candidatePlace.Latitude - averageCoordinates.Y);
                double differenceLongitude = Math.Abs(candidatePlace.Longitude - averageCoordinates.X);
                double differenceBoth = differenceLatitude + differenceLongitude;
                if (differenceBoth < Math.Abs(initialDifference.Y + initialDifference.X))
                    finalPlace = candidatePlace;
            }
            if (finalPlace == null)
                finalPlace = await this.DefaultBehaviour(geoCoordinates, placesProvider, placesTypes, 1000);
            return finalPlace;
        }
    }
}
