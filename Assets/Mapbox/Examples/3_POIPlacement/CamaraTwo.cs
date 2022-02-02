using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraTwo : MonoBehaviour
{
    public class ZoomCamera : MonoBehaviour
    {
        [Range(0.1f, 2)]
        public float speedZoom = 1f;
        private Camera cam;
        [Header("Zoom Limits")]
        public float minZoom = 2f;
        public float maxZoom = 20f;
        [Header("X Move Limits")]
        public float minMoveX = -10f;
        public float maxMoveX = 10f;
        [Header("Y Move Limits")]
        public float minMoveY = -10f;
        public float maxMoveY = 10f;
        [Header("Z Move Limits")]
        public bool moveInZ = false;
        public float minMoveZ = 0f;
        public float maxMoveZ = 0f;

        private Vector2 startPosPointer, lastPosPointer;//guarda la primera y ultima posicion del puntero para mover la camara
        private float touchesDistanceMoved = 0f, touchesDistanceStationary = 0f;//guarda la distancia entre los dos touch en dispositivos tactiles
        public static bool isZoom = false;//se utiliza para detectar si se esta haciendo zoom o si se esta moviendo la camara
        private float multX = 0f, multY = 0f;//multiplicador del valor del puntero para que la camara siga al puntero
        private float screenWith = 0f, screenHeight = 0f;
        private bool pointerDown = false;//funciona para que siempre se detecte cuando se empieza a interactuar con el movimiento de la camara
        private Vector3 posTouch0, posTouch1;
        private bool zoomScrollMouse = false;

        void Start()
        {
            cam = GetComponent<Camera>();
            screenWith = System.Convert.ToSingle(Screen.width);
            screenHeight = System.Convert.ToSingle(Screen.height);
            if (cam.orthographic)
            {//se obtiene el valor del multiplicador de la posicion del puntero en vista ortografica
                multX = (screenWith / (screenHeight / 2));
                multY = 2f;
            }
            else
            {//se obtiene el valor del multiplicador de la posicion del puntero en vista perspectiva
                multX = ((screenWith / screenHeight) / 5.49f);
                multY = ((screenHeight / screenWith) * multX); ;
            }

        }

        void FixedUpdate()
        {
           // if //(//condicion para no hacer zoom o mover la camara)
        {//este if hace que por ejemplo cuando se muestre el menu no se haga zoom  o se mueva la camara mientras se muestre algo
                ZoomAndMoveCameraController();
            }

        }

        private void ZoomAndMoveCameraController()
        {
            //=============== ZOOM O MOVIMIENTO DE LA CAMARA CON EL MOUSE ==============================================
            // se encarga de hacer zoom con con orthographicSize o el fieldView de la camara con el scroll del mouse
            // Zoom ortografica
            if (cam.orthographic)
            {
                if (Input.mouseScrollDelta.y > 0 && cam.orthographicSize > minZoom)
                {//acerca
                    cam.orthographicSize -= Input.mouseScrollDelta.y * (0.5f * speedZoom);
                    isZoom = true;
                    zoomScrollMouse = true;
                }
                else if (Input.mouseScrollDelta.y < 0 && cam.orthographicSize < maxZoom)
                {//aleja
                    cam.orthographicSize -= Input.mouseScrollDelta.y * (0.5f * speedZoom);
                    isZoom = true;
                    zoomScrollMouse = true;
                }
            }
            else
            {//Zoom perspectiva
                if (Input.mouseScrollDelta.y > 0 && cam.fieldOfView > minZoom)
                {//acerca
                    cam.fieldOfView -= Input.mouseScrollDelta.y * (0.75f * speedZoom);
                    isZoom = true;
                    zoomScrollMouse = true;
                }
                else if (Input.mouseScrollDelta.y < 0 && cam.fieldOfView < maxZoom)
                {//aleja
                    cam.fieldOfView -= Input.mouseScrollDelta.y * (0.75f * speedZoom);
                    isZoom = true;
                    zoomScrollMouse = true;
                }

            }
            if (Input.mouseScrollDelta.y == 0 && isZoom && zoomScrollMouse)
            {
                isZoom = false;
                zoomScrollMouse = false;
            }


            if (Input.GetMouseButtonDown(2))
            {
                startPosPointer = lastPosPointer = new Vector2(cam.ScreenToViewportPoint(Input.mousePosition).x, cam.ScreenToViewportPoint(Input.mousePosition).y);
                pointerDown = true;
                isZoom = true;
            }
            else if (Input.GetMouseButton(2) && pointerDown)
            {
                // se encarga de mover la camara segun el movimiento del puntero
                startPosPointer = new Vector2(cam.ScreenToViewportPoint(Input.mousePosition).x, cam.ScreenToViewportPoint(Input.mousePosition).y);
                if (Vector2.Distance(startPosPointer, lastPosPointer) > 0.003f)
                {
                    MoveCamera();//para mover la camara segun la posicion del puntero
                    lastPosPointer = new Vector2(cam.ScreenToViewportPoint(Input.mousePosition).x, cam.ScreenToViewportPoint(Input.mousePosition).y);
                }

            }
            else if (Input.GetMouseButtonUp(2))
            {
                pointerDown = false;
                isZoom = false;
            }

            //=============== ZOOM O MOVIMIENTO DE LA CAMARA CON DOS TOUCHES ==============================================
            if (Input.touchCount == 2)
            {
                isZoom = true;
                if (Input.GetTouch(1).phase == TouchPhase.Began)
                {
                    pointerDown = true;
                    posTouch0 = cam.ScreenToViewportPoint(Input.GetTouch(0).position);
                    posTouch1 = cam.ScreenToViewportPoint(Input.GetTouch(1).position);
                    startPosPointer = lastPosPointer = new Vector2((posTouch0.x + posTouch1.x) / 2, (posTouch0.y + posTouch1.y) / 2);
                    touchesDistanceMoved = touchesDistanceStationary = (Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) - Vector2.Distance(Input.GetTouch(0).deltaPosition, Input.GetTouch(1).deltaPosition)) / (100f / speedZoom);
                }
                else if ((Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) && pointerDown)
                {

                    posTouch0 = cam.ScreenToViewportPoint(Input.GetTouch(0).position);
                    posTouch1 = cam.ScreenToViewportPoint(Input.GetTouch(1).position);
                    startPosPointer = new Vector2((posTouch0.x + posTouch1.x) / 2, (posTouch0.y + posTouch1.y) / 2);
                    touchesDistanceMoved = (Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) - Vector2.Distance(Input.GetTouch(0).deltaPosition, Input.GetTouch(1).deltaPosition)) / (100f / speedZoom);
                    print(touchesDistanceMoved - touchesDistanceStationary);
                    if (Vector2.Distance(startPosPointer, lastPosPointer) > 0.003f)
                    {
                        // se encarga de hacer zoom con con orthographicSize o el fieldView de la camara segun la separacion de dos touches

                        if (cam.orthographic)
                        {//Zoom
                            if (touchesDistanceMoved > touchesDistanceStationary && cam.orthographicSize > minZoom)
                            {//acercar
                                cam.orthographicSize -= (touchesDistanceMoved - touchesDistanceStationary);
                                touchesDistanceStationary = touchesDistanceMoved;
                            }
                            else if (touchesDistanceMoved < touchesDistanceStationary && cam.orthographicSize < maxZoom)
                            {//alejar
                                cam.orthographicSize += (touchesDistanceStationary - touchesDistanceMoved);
                                touchesDistanceStationary = touchesDistanceMoved;
                            }
                        }
                        else
                        {//Zoom
                            if (touchesDistanceMoved > touchesDistanceStationary && cam.fieldOfView > minZoom)
                            {//acercar
                                cam.fieldOfView -= (touchesDistanceMoved - touchesDistanceStationary);
                                touchesDistanceStationary = touchesDistanceMoved;
                            }
                            else if (touchesDistanceMoved < touchesDistanceStationary && cam.fieldOfView < maxZoom)
                            {//alejar
                                cam.fieldOfView += (touchesDistanceStationary - touchesDistanceMoved);
                                touchesDistanceStationary = touchesDistanceMoved;
                            }
                        }

                        // se encarga de mover la camara segun el movimiento del pnto medio de dos touches
                        MoveCamera();//para mover la camara segun el punto medio de los dos touches

                        posTouch0 = cam.ScreenToViewportPoint(Input.GetTouch(0).position);
                        posTouch1 = cam.ScreenToViewportPoint(Input.GetTouch(1).position);
                        lastPosPointer = new Vector2((posTouch0.x + posTouch1.x) / 2, (posTouch0.y + posTouch1.y) / 2);


                    }

                }

            }

            if (Input.touchCount == 1 && isZoom)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    isZoom = false;
                    pointerDown = false;
                }
            }
        }

        private void MoveCamera()
        {//mueve la camara segun la distancia calculada del movimiento del puntero o el punto medio de dos touches
            float distanceMovedX;
            float distanceMovedY;
            if (cam.orthographic)
            {
                distanceMovedX = (startPosPointer.x - lastPosPointer.x) * (cam.orthographicSize * multX);
                distanceMovedY = (startPosPointer.y - lastPosPointer.y) * (cam.orthographicSize * multY);
            }
            else
            {
                distanceMovedX = (startPosPointer.x - lastPosPointer.x) * (cam.fieldOfView * multX);
                distanceMovedY = (startPosPointer.y - lastPosPointer.y) * (cam.fieldOfView * multY);
            }

            if (moveInZ)
            {//esto sirve para que funcione cuando la camara tiene una rotacion en el eje Y
                cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x - distanceMovedX, minMoveX, maxMoveX), Mathf.Clamp(cam.transform.position.y - distanceMovedY, minMoveY, maxMoveY), Mathf.Clamp(cam.transform.position.z + (distanceMovedX * (cam.transform.rotation.eulerAngles.y / 100)), minMoveZ, maxMoveZ));
            }
            else
            {
                cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x - distanceMovedX, minMoveX, maxMoveX), Mathf.Clamp(cam.transform.position.y - distanceMovedY, minMoveY, maxMoveY), cam.transform.position.z);
            }

        }

    }
}
