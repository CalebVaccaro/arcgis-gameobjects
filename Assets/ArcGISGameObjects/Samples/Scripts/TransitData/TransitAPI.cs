using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class TransitAPI
{
    public static readonly string resourcesFilePath = "TransitGISData";
    public List<TransitLocation> GetBusStops(int count) => GetTransitLocations(TransitType.Bus, count);
    public List<TransitLocation> GetTransitStops(int count) => GetTransitLocations(TransitType.Transit, count);
    public List<TransitLocation> GetPointsOfInterests(int count) => GetTransitLocations(TransitType.Poi, count);

    public static T GetTransitData<T>(TransitType transitType)
    {
        string file = TransitTypeString(transitType);

        string filePath = Path.Join(resourcesFilePath, file);
        TextAsset text = Resources.Load<TextAsset>(filePath);
        return JsonConvert.DeserializeObject<T>(text.text);
    }

    public static List<TransitLocation> GetTransitLocations(TransitType transitType, int count)
    {
        var transitData = GetTransitData<TransitData>(transitType);
        return TransitLocations.GetTransitLocations(transitData, transitType, count);
    }

    public static string TransitTypeString(TransitType transitType)
    {
        return transitType switch
        {
            TransitType.Bus => nameof(TransitType.Bus),
            TransitType.Poi => nameof(TransitType.Poi),
            TransitType.Transit => nameof(TransitType.Transit),
            _ => string.Empty
        };
    }
}
