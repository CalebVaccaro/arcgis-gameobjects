using Esri.GameEngine.Geometry;
using UnityEngine;
using ArcGISGameObjects;
using Esri.ArcGISMapsSDK.Components;

public class CreateObjectsController : MonoBehaviour
{
    [SerializeField] private GameObject arcGISPrefab;
    private Transform arcGISMapParent;
    public void Start()
    {
        arcGISMapParent = FindObjectOfType<ArcGISMapComponent>().gameObject.transform;
    }
    
    public void CreateArcGISGameObject(ArcGISPoint arcGisPoint)
    {
        ArcGISGameObject.CreateArcGISGameObject(arcGISPrefab, arcGISMapParent, arcGisPoint);
    }
}
