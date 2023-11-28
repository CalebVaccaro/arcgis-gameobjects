using UnityEngine;
using System.Collections.Generic;
using ArcGISGameObjects;

public class NYCOpenDataController : MonoBehaviour
{
    [SerializeField] private GameObject arcGISPrefab;
    [SerializeField] private int queryCount = 10;
    [SerializeField] private double altitude = 40;
    private List<ArcGISGameObject> arcGISGameObjects;

    public void Start()
    {
        var parent = GameObject.Find("ArcGISMap").transform;
        arcGISGameObjects = new List<ArcGISGameObject>();
        
        var nycOpenDataApi = new NYCOpenDataAPI();

        // Create Homebase (Homeless Shelter Offices)
        var homebasePositions = nycOpenDataApi.GetHomelessOffices();
        for (int i = 0; i < queryCount; i++)
        {
            var nycDataPosition = ArcGISGameObject.CreateArcGISGameObject(arcGISPrefab, parent);
            nycDataPosition.SetPosition(homebasePositions[i]);
            nycDataPosition.SetAltitude(altitude);
        }

        // Create Wifi Hot Spot Positions
        var wifiPositions = nycOpenDataApi.GetWifiLocations();
        for(int i = 0; i < queryCount; i++)
        {
            var wifiPosition = ArcGISGameObject.CreateArcGISGameObject(arcGISPrefab, parent);
            wifiPosition.SetPosition(wifiPositions[i]);
            wifiPosition.SetAltitude(altitude);
            arcGISGameObjects.Add(wifiPosition);
        }
    }
}