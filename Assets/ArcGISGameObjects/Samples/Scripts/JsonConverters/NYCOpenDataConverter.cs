using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class NYCOpenDataConverter : JsonConverter<LocationData>
{
    public override LocationData ReadJson(JsonReader reader, Type objectType, LocationData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        var locationData = new LocationData();
        
        // Check for 'the_geom' or 'location'
        if (obj["the_geom"] != null)
        {
            locationData.position = obj["the_geom"].ToObject<NYCOpenDataPosition>();
        }
        else if (obj["location"] != null)
        {
            locationData.position = obj["location"].ToObject<NYCOpenDataPosition>();
        }

        // Deserialize other properties as needed
        // ...

        return locationData;
    }

    public override void WriteJson(JsonWriter writer, LocationData value, JsonSerializer serializer)
    {
        // Implement if you need serialization
        throw new NotImplementedException();
    }
}