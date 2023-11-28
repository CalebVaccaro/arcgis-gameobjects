using System.Collections.Generic;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using UnityEngine;
using ArcGISGameObjects;

public class TransitController : MonoBehaviour
{
    private Transform parent;
    public SceneType sceneType;
    [SerializeField] private GameObject transitPrefab;
    [SerializeField] private GameObject busStopPrefab;
    [SerializeField] private GameObject poiPrefab;
    [SerializeField] private Material transitMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material nearestMaterial;
    [SerializeField] private int busCount = 1;
    [SerializeField] private int poiCount = 1;
    [SerializeField] private int transitCount = 1;
    [SerializeField] private double altitude = 10;
    Dictionary<TransitType, List<ArcGISGameObject>> allArcGISGameObjects;
    [SerializeField] private List<ArcGISGameObject> transitStopObjects;

    private ArcGISGameObject nearestGameObject;
    private ArcGISGameObject selectedGameObject;
    private ArcGISGameObject transitSelectedGameObject;

    [SerializeField] private TransitCanvas transitCanvas;

    public void Awake()
    {
        // Find Parent GameOBject to add all children ArcGameObjects
        parent = GameObject.Find("ArcGISMap").transform;
    }

    public void Start()
    {
        allArcGISGameObjects = new Dictionary<TransitType, List<ArcGISGameObject>>();

        // Setup API
        TransitAPI api = new TransitAPI();

        // Get Bus Stops
        var busStops = api.GetBusStops(busCount);
        AddArcGISGameObjectsToMap(busStopPrefab, busStops, TransitType.Bus);

        // Get Point of Interests
        var pointOfInterests = api.GetPointsOfInterests(poiCount);
        AddArcGISGameObjectsToMap(poiPrefab, pointOfInterests, TransitType.Poi);

        // Get Transit Stops
        var transitStops = api.GetTransitStops(transitCount);
        AddArcGISGameObjectsToMap(transitPrefab, transitStops, TransitType.Transit);

        // Get Transit Objects to Hover
        transitStopObjects = allArcGISGameObjects[TransitType.Transit];
    }

    public void AddArcGISGameObjectsToMap(GameObject prefab, List<TransitLocation> transitLocations, TransitType transitType)
    {
        var arcGISGameObjects = new List<ArcGISGameObject>();
        foreach (var transitLocation in transitLocations)
        {
            var position = transitLocation.position;
            var positionWithAltitude = new ArcGISPoint(position.X, position.Y, altitude);
            var rotation = new ArcGISRotation(0, 0, 0);

            // Create TransitArcGISGameObject
            var transitArcGISGameObject = ArcGISGameObject
                .CreateArcGISGameObject<TransitArcGISGameObject>(prefab, parent);

            // Set Position
            transitArcGISGameObject.SetPosition(positionWithAltitude);

            // Set Rotation
            transitArcGISGameObject.SetRotation(rotation);

            // Add TransitInfo
            var transitInfo = transitArcGISGameObject.transitInfo;
            transitInfo = transitLocation.info;

            // Add OnMouse Event for Transit Stops
            if (transitInfo.transitType == TransitType.Transit && sceneType == SceneType.Haversine)
            {
                transitArcGISGameObject.onMouseEvent.AddListener(() =>
                {
                    // Set Selected, Reset Materials
                    if (selectedGameObject != null)
                    {
                        SetMaterial(selectedGameObject.gameObject, transitMaterial);
                    }
                    if (nearestGameObject != null)
                    {
                        SetMaterial(nearestGameObject.gameObject, transitMaterial);
                    }
                    selectedGameObject = transitArcGISGameObject;
                    SetMaterial(transitArcGISGameObject.gameObject, selectedMaterial);
                    
                    // Get Nearest Neighbor using Haversine Algo
                    var nearestNeighbor = Haversine.GetNearestNeighbor(transitArcGISGameObject, arcGISGameObjects);
                    nearestGameObject = nearestNeighbor.nearestGameObject;
                    SetMaterial(nearestGameObject.gameObject, nearestMaterial);

                    // Set Distance Information on Canvas
                    if (transitCanvas != null){
                        var selectedLocation = selectedGameObject.ArcGisLocationComponent.Position;
                        var nearestNeighborLocation = nearestGameObject.ArcGisLocationComponent.Position;
                        var distanceData = new TransitCanvas.DistanceData(
                            selectedLocation.X,
                            selectedLocation.Y,
                            nearestNeighborLocation.X,
                            nearestNeighborLocation.Y,
                            nearestNeighbor.distance);
                        transitCanvas.SetCanvasInfo(distanceData);
                    }
                });
            }

            if (transitCanvas != null && sceneType == SceneType.LocalJSON)
            {
                // Add TransitInfo OnMouseHover Event
                transitArcGISGameObject.onMouseEvent.AddListener(() =>
                {
                    transitCanvas.SetCanvasInfo(transitInfo);
                });
            }

            arcGISGameObjects.Add(transitArcGISGameObject);
        }

        if (allArcGISGameObjects.ContainsKey(transitType))
        {
            allArcGISGameObjects.Remove(transitType);
        }
        allArcGISGameObjects.Add(transitType, arcGISGameObjects);
    }

    void Update()
    {
        var cameraOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cameraOrigin, out var hit))
        {
            var arcGISGameObject = hit.transform.GetComponent<ArcGISGameObject>();
            if (transitSelectedGameObject == null)
            {
                transitSelectedGameObject = arcGISGameObject;
            }
            if (arcGISGameObject)
            {
                transitSelectedGameObject = arcGISGameObject;
                arcGISGameObject.onMouseEvent.Invoke();
            }
        }
    }

    public void SetMaterial(GameObject obj, Material mat)
    {
        obj.GetComponent<Renderer>().material = mat;
    }
}
