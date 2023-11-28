using System;
using System.Collections.Generic;
using System.Linq;
using Esri.GameEngine.Geometry;

public enum TransitType
{
    Transit,
    Bus,
    Poi
}

public record TransitLocation
{
    public ArcGISPoint position { get; set; }
    public TransitInfo info { get; set; }
}

public class TransitLocations
{
    public static List<TransitLocation> GetTransitLocations(
        TransitData transitData,
        TransitType transitType,
        int queryCount
    )
    {
        var locationCollection = transitData.data.GetRange(0, queryCount);
        var transitLocations = locationCollection.Select(location => GetTransitLocationData(location, transitType)).ToList();
        return transitLocations;
    }

    public static TransitLocation GetTransitLocationData(string[] transitDataSet, TransitType transitType)
    {
        var jsonPosition = GetPositionJSONIndex(transitDataSet, transitType);
        var position = ParsePosition(jsonPosition);
        var transitInfo = GetTransitInfo(transitDataSet, transitType);
        return new TransitLocation() { position = new ArcGISPoint(position.X, position.Y), info = transitInfo };
    }

    public static string GetPositionJSONIndex(string[] data, TransitType transitType)
    {
        return transitType switch
        {
            TransitType.Bus => data[11],
            TransitType.Transit => data[11],
            TransitType.Poi => data[9],
            _ => throw new ArgumentOutOfRangeException(nameof(TransitType), transitType, null)
        };
    }

    public static ArcGISPoint ParsePosition(string data)
    {
        var removeParaStart = data.Split('(')[1];
        var removeParaEnd = removeParaStart.Split(')')[0];
        var positionRow = removeParaEnd.Split(' ');

        var parsedLAT = double.Parse(positionRow[1]);
        var parsedLONG = double.Parse(positionRow[0]);

        var roundTo = 8;

        var roundedLAT = Math.Round(parsedLAT, roundTo);
        var roundedLONG = Math.Round(parsedLONG, roundTo);

        return new ArcGISPoint(roundedLONG, roundedLAT);
    }

    // transitJSONColumns: 15 (Street Name), 16(Next Stop), 17(Lat Next Stop), 18(Long Next Stop), 21 (Borue), 23 (Street Name)
    public static TransitInfo GetTransitInfo(string[] data, TransitType transitType)
    {
        return transitType switch
        {
            TransitType.Bus => new TransitInfo(transitType, data[15], data[16], data[21]),
            TransitType.Transit => new TransitInfo(transitType, data[10], data[12], data[13]),
            TransitType.Poi => new TransitInfo(transitType, data[20], data[23], ""),
            _ => throw new ArgumentOutOfRangeException(nameof(TransitType), transitType, null)
        };
    }
}
