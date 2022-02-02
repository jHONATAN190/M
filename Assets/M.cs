using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;

public class M : MonoBehaviour
{
   // public GameObject Objeto1;
    public GameObject prefab;
   public  AbstractMap map;
   MapLocationOptions mapLocationOptions = new MapLocationOptions();

    void Start()
    {
        //Instantiate(Objeto1);
        //Instantiate(prefab, transform.position, transform.rotation);

    }

    
   public void GoToMedellin()
    {
        map.Options.locationOptions.latitudeLongitude = "3.476929903402999, -76.52443564239535";
        map.SetUpMap();


    }
}
