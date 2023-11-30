using UnityEngine;
using UnityEngine.UI;

public class TransitCanvas : MonoBehaviour
{
    public Text name;
    public Text street;
    public Text frequency;
    public Text transitType;

    public void SetCanvasInfo(TransitInfo transitInfo)
    {
        name.text = transitInfo?.transitName;
        street.text = transitInfo?.streetName;
        frequency.text = transitInfo?.frequency;
        if (transitInfo != null) transitType.text = TransitAPI.TransitTypeString(transitInfo.transitType);
    }

    public void SetCanvasInfo(DistanceData data)
    {
        name.text = $"{data.X1} : {data.Y1}";
        street.text = $"{data.X2} : {data.Y2}";
        frequency.text = $"{data.Distance}";
        transitType.text = "";
    }

    public struct DistanceData
    {
        public string X1;
        public string Y1;
        public string X2;
        public string Y2;
        public string Distance;

        public DistanceData(double x1, double y1, double x2, double y2, double distance)
        {
            X1 = x1.ToString();
            Y1 = y1.ToString();
            X2 = x2.ToString();
            Y2 = y2.ToString();
            Distance = distance.ToString();
        }
    }
}
