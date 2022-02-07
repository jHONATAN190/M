using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcultarActivar : MonoBehaviour
{
    public GameObject objeto;
    void Start()
    {
        
    }

    public void MostrarObjeto()

    {
        if (objeto.activeSelf == false)
            objeto.SetActive(true);
        else
            objeto.SetActive(false);
    }
    
}
