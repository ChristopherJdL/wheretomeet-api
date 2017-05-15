using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class PerfectPlaceAlgorithm
    {
        PerfectPlaceAlgorithm()
        {

        }
        Place FindPerfectPlace(IPlaceProvider placeProvider, String[] placeType,
            IDistanceResolver distanceResolver,GeoCoordinates[] geoCoordinates)
        {
            double sumX=0.0, sumY=0.0;
            foreach(GeoCoordinates item in geoCoordinates) {
                sumX+=item.x;
                sumY+=item.y;
                num
            }

            GeoCoordinates averageCoordinates = new GeoCoordinates();
            averageCoordinates.x = sumX/geoCoordinates.Length;
            averageCoordinates.y = sumY/geoCoordinates.Length;

            Place[] candidatePlace = placeProvider.FindNearbyPlaces(averageCoordinates,500,placeType);

            int minimumMinute=0;
            Place finalPlace=null;
            foreach(Place item1 in candidatePlace) {
                int minute=0;
                foreach(GeoCoordinates item2 in geoCoordinates){
                    minute+=distanceResolver.ResloveDuration(item1.Location, item2);
                }
                if(finalPlace==null || minimumMinute<minute){
                    minimumMinute=minute;
                    finalPlace=item1;
                }
            }

            return finalPlace;
        }
    }
}
