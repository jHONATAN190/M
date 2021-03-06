using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    /* To turn off the visibility 
     * of the float inputs in the editor 
     * just delete [SerializeField] 
     * from the beginning of each float line */

    public Transform cameraTransform;

    [SerializeField] private float _camSpeed = 1f; //Speed of the camera
    [SerializeField] private float _camSpeedFast = 5f; //Speed of the camera while holding "Fast camera movement button"

    [SerializeField] private float _camMovementSpeed = 1f;
    [SerializeField] private float _camSmoothness = 10f;

    [SerializeField] private float _camRotationAmount = 1f;
    [SerializeField] private float _camBorderMovement = 5f;

    [SerializeField] private float _maxCamZoom = 10f;
    [SerializeField] private float _minCamZoom = 100f;

    [SerializeField] private float _minZCamMovement = 100f;
    [SerializeField] private float _maxZCamMovement = 900f;
    [SerializeField] private float _minXCamMovement = 100f;
    [SerializeField] private float _maxXCamMovement = 900f;

    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    //MouseMovement
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    public float zoomSensitivity = 15;
    public float swipeSensitivity = 15;

    public float swipeThreshold = 50f;
    public float timeThreshold = 0.3f;

   /* public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;
    public UnityEvent OnSwipeUp;
    public UnityEvent OnSwipeDown;*/

    private Vector2 fingerDown;
    private DateTime fingerDownTime;
    private Vector2 fingerUp;
    private DateTime fingerUpTime;

    

// Start is called before the first frame update
void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetMouseButtonDown(0))
    {
        this.fingerDown = Input.mousePosition;
        this.fingerUp = Input.mousePosition;
        this.fingerDownTime = DateTime.Now;
    }
    if (Input.GetMouseButtonUp(0))
    {
        this.fingerDown = Input.mousePosition;
        this.fingerUpTime = DateTime.Now;
        this.CheckSwipe();
    }
    /*foreach (Touch touch in Input.touches)
    {
        if (touch.phase == TouchPhase.Began)
        {
            this.fingerDown = touch.position;
            this.fingerUp = touch.position;
            this.fingerDownTime = DateTime.Now;
        }
        if (touch.phase == TouchPhase.Ended)
        {
            this.fingerDown = touch.position;
            this.fingerUpTime = DateTime.Now;
            this.CheckSwipe();
        }
    }*/
         HandleMovementInput();
        HandleMouseInput();
    }
    private void CheckSwipe()
    {
        float duration = (float)this.fingerUpTime.Subtract(this.fingerDownTime).TotalSeconds;
        if (duration > this.timeThreshold) return;

        float deltaX = this.fingerDown.x - this.fingerUp.x;
        if (Mathf.Abs(deltaX) > this.swipeThreshold)
        {
            if (deltaX > 0)
            {
              //  this.OnSwipeRight.Invoke();
                Debug.Log("right");
                newPosition += (transform.right * -_camMovementSpeed) * swipeSensitivity;

            }
            else if (deltaX < 0)
            {
               // this.OnSwipeLeft.Invoke();
                Debug.Log("left");
                newPosition += (transform.right * _camMovementSpeed) * swipeSensitivity;

            }
        }

        float deltaY = fingerDown.y - fingerUp.y;
        if (Mathf.Abs(deltaY) > this.swipeThreshold)
        {
            if (deltaY > 0)
            {
              //  this.OnSwipeUp.Invoke();
                Debug.Log("up");
                newPosition += (transform.up * -_camMovementSpeed) * swipeSensitivity;

            }
            else if (deltaY < 0)
            {
              //  this.OnSwipeDown.Invoke();
                Debug.Log("down");
                newPosition += (transform.up * _camMovementSpeed) * swipeSensitivity;
            }
        }

        this.fingerUp = this.fingerDown;
    }
    void HandleMouseInput()
    {
        //Scroll zooming
        if (Input.mouseScrollDelta.y != 0)
        {

           /* newZoom += Input.mouseScrollDelta.y * zoomAmount;

            if (newZoom.y <= 30) //Max zoom limit
            {
               newZoom = new Vector3(0, 30, -30);

            } else if (newZoom.y >= 220) //Min zoom limit
            {
                newZoom = new Vector3(0, 120, -120);
            }*/
            newPosition += (transform.forward * Input.mouseScrollDelta.y )* zoomSensitivity;
        }

        //Camera rotating on mouse scroll button hold

        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.right * (-difference.x / 5));
        }

    }

    void HandleMovementInput()

        //Fast camera movement
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _camMovementSpeed = _camSpeedFast;
        }
        else
        {
            _camMovementSpeed = _camSpeed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) /*|| Input.mousePosition.y >= Screen.height - _camBorderMovement*/)
        {
            newPosition += (transform.up * _camMovementSpeed);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) /*|| Input.mousePosition.y <= _camBorderMovement*/)
        {
            newPosition += (transform.up * -_camMovementSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)/* || Input.mousePosition.x >= Screen.width - _camBorderMovement*/)
        {
            newPosition += (transform.right * _camMovementSpeed);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)/* || Input.mousePosition.x <= _camBorderMovement*/)
        {
            newPosition += (transform.right * -_camMovementSpeed);
        }

        //Keyboard setup for camera rotate
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * _camRotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -_camRotationAmount);
        }

        //Keyboard setup for camera zoom
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;

            //Max zoom limit
            if (newZoom.y <= 30)
            {
                newZoom = new Vector3(0, 30, -30);

            }
            
        }

        //Min zoom limit
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
            if (newZoom.y >= 120)
            {
                newZoom = new Vector3(0, 120, -120);
            }
        }

        //Setting Borders
        if (newPosition.x < _minXCamMovement)
        {
            newPosition = new Vector3(_minXCamMovement, transform.position.y, transform.position.z);

        } else if(newPosition.x > _maxXCamMovement)
        {
            newPosition = new Vector3(_maxXCamMovement, transform.position.y, transform.position.z);
        }

        if (newPosition.z < _minZCamMovement)
        {
            newPosition = new Vector3(transform.position.x, transform.position.y, _minZCamMovement);

        }
        else if (newPosition.z > _maxZCamMovement)
        {
            newPosition = new Vector3(transform.position.x, transform.position.y, _maxZCamMovement);
        }


        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _camSmoothness);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * _camSmoothness);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * _camSmoothness);
    }

   
}
