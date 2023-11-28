using System;
using System.Collections.Generic;
using ArcGISGameObjects;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;

// HAVERSINE REFERENCE SCRIPT: https://gist.github.com/jammin77/033a332542aa24889452
/// <summary>
/// The distance type to return the results in.
/// </summary>
static class Haversine
{
    public enum DistanceType { Miles, Kilometers };

    /// <summary>
    /// Returns the distance in miles or kilometers of any two
    /// latitude / longitude points.
    /// </summary>
    /// <param name=”pos1″></param>
    /// <param name=”pos2″></param>
    /// <param name=”type”></param>
    /// <returns></returns>
    private static double Distance(ArcGISPoint pos1, ArcGISPoint pos2, DistanceType type = DistanceType.Kilometers)
    {
        // Get Distance Ref Type
        double R = (type == DistanceType.Miles) ? 3960 : 6371;

        // Get Latitude Difference
        double dLat = toRadian(pos2.Y - pos1.Y);
        // Get Longitude Difference
        double dLon = toRadian(pos2.X - pos1.X);

        // accumulative difference in latitude and longitude
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(toRadian(pos1.Y)) * Math.Cos(toRadian(pos2.Y)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        // Output distance
        double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));

        // Reference * Output = (Double) Distance between geolocations
        double d = R * c;

        return d;
    }

    /// <summary>
    /// Convert to Radians.
    /// </summary>
    /// <param name=”val”></param>
    /// <returns></returns>
    private static double toRadian(double val)
    {
        return (Math.PI / 180) * val;
    }

    /// <summary>
    /// Bubble Sort to find nearest neighbor
    /// </summary>
    /// <param name="selectedArcGISGameObject"></param>
    /// <param name="objectsToSearch"></param>
    /// <returns></returns>
    public static (ArcGISGameObject nearestGameObject, double distance) GetNearestNeighbor(ArcGISGameObject selectedArcGISGameObject, List<ArcGISGameObject> objectsToSearch)
    {
        var selectedPoint = selectedArcGISGameObject.GetComponent<ArcGISLocationComponent>().Position;
        var distance = 0.0000f;
        
        // Precompute distances and positions
        double[] distances = new double[objectsToSearch.Count];
        for (int i = 0; i < objectsToSearch.Count; i++)
        {
            var arcGISPosition = objectsToSearch[i].GetComponent<ArcGISLocationComponent>().Position;
            distances[i] = Distance(selectedPoint, arcGISPosition);
        }

        // Bubble Sort with early termination
        bool swapped;
        for (int i = 0; i < objectsToSearch.Count; i++)
        {
            swapped = false;
            for (int j = 0; j < objectsToSearch.Count - 1 - i; j++)
            {
                if (distances[j] > distances[j + 1])
                {
                    // Swap distances
                    (distances[j], distances[j + 1]) = (distances[j + 1], distances[j]);

                    // Swap objects in list
                    (objectsToSearch[j], objectsToSearch[j + 1]) = (objectsToSearch[j + 1], objectsToSearch[j]);

                    swapped = true;
                }
            }
            // If no two elements were swapped by inner loop, break
            if (!swapped) break;
        }

        return (objectsToSearch[1], distances[1]);
    }
}