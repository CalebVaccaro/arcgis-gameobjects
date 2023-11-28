using System;
using Esri.GameEngine.Geometry;
using UnityEngine;
using UnityEngine.UI;

public class CreateCanvas : MonoBehaviour
{
    public InputField LONG_IF;
    public InputField LAT_IF;
    public InputField ALT_IF;
    public Button CreateObject_Button;
    public CreateObjectsController controller;
    
    public void Start()
    {
        CreateObject_Button.onClick.AddListener(CreateArcGISGameObject);
    }

    public void CreateArcGISGameObject()
    {
        try{
            var longitude = double.Parse(LONG_IF.text);
            var latitude = double.Parse(LAT_IF.text);
            var altitude = double.Parse(ALT_IF.text);
            
            var position = new ArcGISPoint(longitude, latitude, altitude);
            controller.CreateArcGISGameObject(position);
        }
        catch (Exception e){
            Debug.Log("Cannot Parse Input Position");
            throw;
        }

        LONG_IF.text = "";
        LAT_IF.text = "";
        ALT_IF.text = "";
    }
}