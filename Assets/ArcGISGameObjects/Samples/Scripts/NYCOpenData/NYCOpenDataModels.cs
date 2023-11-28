using Newtonsoft.Json;

#region NYCOpenData

[JsonConverter(typeof(NYCOpenDataConverter))]
public class LocationData
{
    public NYCOpenDataPosition position { get; set; }
    // ... other properties as per the JSON structure
}

public class NYCOpenDataPosition
{
    public string type { get; set; }
    public double[] coordinates { get; set; }
}

#endregion

