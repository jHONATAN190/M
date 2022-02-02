using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M : MonoBehaviour
{
   // public GameObject Objeto1;
    public GameObject prefab;
    

    void Start()
    {
        //Instantiate(Objeto1);
        Instantiate(prefab, transform.position, transform.rotation);

    }

    
    void Update()
    {
        
    }
}
