public class TransitInfo
{
    public string transitName { get; set; }
    public string streetName { get; set; }
    public string frequency { get; set; }
    public TransitType transitType { get; set; }

    public TransitInfo(TransitType transitType, string name, string streetName, string frequency)
    {
        this.transitType = transitType;
        this.transitName = name;
        this.streetName = streetName;
        this.frequency = frequency;
    }
}