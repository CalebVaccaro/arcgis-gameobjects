using System.Collections.Generic;

#region TransitData

public class TransitData
{
    public List<string[]> data;
    public object meta;
}

public enum SceneType
{
    LocalJSON,
    Haversine
}

#endregion
