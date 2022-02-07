using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HospitalDescription : MonoBehaviour
{
    public Text descriptionText;
   public GameObject description;

    //public string textvalue;
    //public Text textelement;

    void Start()
    {
        
    }

    /* void update()
    {
        textelement.text = textvalue;
    }
    */

    void OnMouseDown()
    {
        // this object was clicked - do something
        description.SetActive(true);
       descriptionText.text = "Avenida Caracas # 49-83 Local 2, En frente a la estación de transmilenio Marly \n  3314550 EXT: 14517 Y 14518Celular: 3163334341  \n  Horarios en la sede \n Rayos x \nLunes a Viernes de 6am a 9pm, Sabados de 6:30 a 6pm \n Mamografía \n Lunes a Viernes de 6:30am a 8:30pm, Sabados de 7am a 5pm";
    }
}
