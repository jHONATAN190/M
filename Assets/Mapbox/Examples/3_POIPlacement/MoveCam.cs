using UnityEngine;
using System.Collections;

public class MoveCam : MonoBehaviour {

    Vector2 touchDeltaPosition;

    void Update () {
       /* if (Input.GetMouseButton(0))
        {
            float pointer_x = Input.GetAxis("Mouse X");
            float pointer_y = Input.GetAxis("Mouse Y");
            transform.Translate(-pointer_x * 0.5f,
                        -pointer_y * 0.5f, 0);
        }
        */
        if (Input.touchCount == 1)
        {
            Touch touchZero = Input.GetTouch(0);
            if (touchZero.phase == TouchPhase.Moved)
            {
                touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                gameObject.transform.Rotate(touchDeltaPosition.y * .05f, -touchDeltaPosition.x * .4f, 0);
            }
        }
    }
}
