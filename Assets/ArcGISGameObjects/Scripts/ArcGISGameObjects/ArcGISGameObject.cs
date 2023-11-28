using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using UnityEngine;
#if UNITY_EDITOR
#endif
using UnityEngine.Events;

namespace ArcGISGameObjects
{
    public class ArcGISGameObject : MonoBehaviour, IArcGISGameObject
    {
        public ArcGISLocationComponent ArcGisLocationComponent
        {
            get { return arcGISLocationComponent;}
            set { arcGISLocationComponent = value; }
        }

        private ArcGISLocationComponent arcGISLocationComponent;
        public UnityEvent onMouseEvent;

        public void SetPosition(ArcGISPoint position)
        {
            arcGISLocationComponent.Position =
                new ArcGISPoint(position.X, position.Y, position.Z, ArcGISSpatialReference.WGS84());
        }

        public void SetAltitude(double altitude)
        {
            var position = arcGISLocationComponent.Position;
            arcGISLocationComponent.Position =
                new ArcGISPoint(position.X, position.Y, altitude, ArcGISSpatialReference.WGS84());
        }

        public void SetRotation(ArcGISRotation rotation)
        {
            arcGISLocationComponent.Rotation = rotation;
        }

        public static ArcGISGameObject CreateArcGISGameObject(GameObject arcGISPrefab, Transform transformParent)
        {
            var arcGisPrefabInstance = Instantiate(arcGISPrefab, Vector3.zero, Quaternion.identity, transformParent);

            var arcGISGameObject = arcGisPrefabInstance.GetComponent<ArcGISGameObject>();
            arcGISGameObject.arcGISLocationComponent = arcGisPrefabInstance
                .GetComponent<ArcGISLocationComponent>();
            return arcGISGameObject;
        }

        // Generic method
        public static T CreateArcGISGameObject<T>(GameObject arcGISPrefab, Transform transformParent)
            where T : ArcGISGameObject
        {
            var arcGisPrefabInstance = Instantiate(arcGISPrefab, Vector3.zero, Quaternion.identity, transformParent);

            var arcGISGameObject = arcGisPrefabInstance.GetComponent<T>();
            if (arcGISGameObject != null){
                arcGISGameObject.arcGISLocationComponent = arcGisPrefabInstance.GetComponent<ArcGISLocationComponent>();
            }

            return arcGISGameObject;
        }
        
        public static ArcGISGameObject CreateArcGISGameObject(GameObject arcGISPrefab, Transform transformParent, ArcGISPoint position)
        {
            var arcGisPrefabInstance = Instantiate(arcGISPrefab, Vector3.zero, Quaternion.identity, transformParent);

            var arcGISGameObject = arcGisPrefabInstance.GetComponent<ArcGISGameObject>();
            arcGISGameObject.arcGISLocationComponent = arcGisPrefabInstance
                .GetComponent<ArcGISLocationComponent>();
            
            arcGISGameObject.SetPosition(position);
            
            return arcGISGameObject;
        }
    }
}
