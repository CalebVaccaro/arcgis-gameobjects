using Esri.ArcGISMapsSDK.Components;

namespace ArcGISGameObjects
{
    public interface IArcGISGameObject
    {
        public ArcGISLocationComponent ArcGisLocationComponent { get; set; }
    }
}