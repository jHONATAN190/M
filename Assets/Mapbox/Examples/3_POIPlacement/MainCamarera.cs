using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamarera : MonoBehaviour
{
    public float speedDH;
    public float speedDt;

    float yaw;
    float picth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yaw += speedDH * Input.GetAxis("Mouse x");
        picth -= speedDt * Input.GetAxis("Mouse y");

        transform.eulerAngles = new Vector3(picth, yaw, 0.0f);
    }
}
