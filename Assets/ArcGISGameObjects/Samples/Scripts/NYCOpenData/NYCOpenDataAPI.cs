using Esri.GameEngine.Geometry;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class NYCOpenDataAPI
{
    public static readonly string homelessOfficesURL = "https://data.cityofnewyork.us/resource/693u-uax6.json";
    public static readonly string wifiLocationsURL = "https://data.cityofnewyork.us/resource/varh-9tsp.json";
    public List<ArcGISPoint> GetHomelessOffices() => GetArcGISPoints(homelessOfficesURL);
    public List<ArcGISPoint> GetWifiLocations() => GetArcGISPoints(wifiLocationsURL);
    
    // NYC Open Data does not contain altitude; set default
    public static double altitude = 10;

    public static List<ArcGISPoint> GetArcGISPoints(string url)
    {
        var data = GetGISData<LocationData[]>(url);
        var coordinates = data.Select(e => e.position?.coordinates);
        var positions = coordinates
            .Select(e => new ArcGISPoint(e[0], e[1], altitude))
            .ToList();
        return positions;
    }

    public static T GetGISData<T>(string url)
    {
        string jsonData = Get(url);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    // Request Method to return a string from url
    // ie. NYC Transit Stop JSON data
    public static string Get(string uri)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        request.AutomaticDecompression =
            DecompressionMethods.None | DecompressionMethods.GZip | DecompressionMethods.Deflate;

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}