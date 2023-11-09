using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject linkedObject; //the object that the camera is following
    public Rigidbody2D objectRb;
    public int cameraMode; //the mode of the camera - 1: following an object, 2: locked to an object (1 but without adjusting its position based on momentum), 3: locked onto a certain point (can be useful for, like, boss fights)
    [SerializeField] private float maxOffset;

    private float targetX; //targets for the camera offset - these coordinates are where the camera is currently heading towards
    private float targetY;

    private float offsetX; //current camera offset
    private float offsetY;
    Vector3 offset;
    Vector3 currentOffset = Vector3.zero;
    Vector3 targetOffset;

    private float camSpeedX = 0;
    private float camSpeedY = 0;
    [SerializeField] private float camSpeed;

    Vector3 camVelocity = Vector3.zero;

    private float moveDirX;
    private float moveDirY;

    Vector2 boundsTopLeft; //these two variables together form a rectangle that works as the bounds of the camera - regardless of where the player goes, the camera will never move outside of these bounds
    Vector2 boundsBottomRight;
    private void Start()
    {
        cameraMode = 1;
        maxOffset = 2;
        objectRb = linkedObject.GetComponent<Rigidbody2D>();
        moveDirX = Mathf.Sign(objectRb.velocity.x);
        moveDirY = Mathf.Sign(objectRb.velocity.y);


    }


    private void LateUpdate() //using lateupdate so the camera's position updates after the linked object's
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            moveDirX = Mathf.Sign(Input.GetAxisRaw("Horizontal"));
        }
        else
        {
            moveDirX = 0;
        }
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            moveDirY = Mathf.Sign((Input.GetAxisRaw("Vertical")));
        }
        else
        {
            moveDirY = 0;
        }

        switch (cameraMode)
        {
            case 1:
                LooseFollowingCamera();
                break;
            case 2:
                FollowingCamera();
                break;
            case 3:
                FixedCamera();
                break;
            default:
                break;
        }

    }

    void FollowingCamera() //camera mode 2, literally just follows the object with no other stuff - idk why you'd use this but it was so easy to add i figured i might as well
    {
        transform.position = new Vector3(linkedObject.transform.position.x, linkedObject.transform.position.y, -10);
    }

    void LooseFollowingCamera() //camera mode 1
    {
        targetOffset = new Vector3(moveDirX * maxOffset,moveDirY * maxOffset, transform.position.z);

        if (currentOffset.x > targetOffset.x)
        {
            camSpeedX = -camSpeed * Time.deltaTime;
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                camSpeedX -= 3 * Time.deltaTime;
            }
        }
        else if (currentOffset.x < targetOffset.x)
        {
            camSpeedX = camSpeed * Time.deltaTime;
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                camSpeedX += 3 * Time.deltaTime;
            }
        }
        else
        {
            camSpeedX = 0;
        }

        if (currentOffset.y > targetOffset.y)
        {
            camSpeedY = -camSpeed * Time.deltaTime;
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                camSpeedY -= 3 * Time.deltaTime;
            }
        }
        else if (currentOffset.y < targetOffset.y)
        {
            camSpeedY = camSpeed * Time.deltaTime;
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                camSpeedY += 3 * Time.deltaTime;
            }
        }
        else
        {
            camSpeedY = 0;
        }

        currentOffset = new Vector3(currentOffset.x + camSpeedX, currentOffset.y + camSpeedY, transform.position.z);

        transform.position = linkedObject.transform.position + currentOffset;


        //if (target.x > transform.position.x)
        //{
        //    camSpeedX = 2 * Time.deltaTime;

        //}
        //else if (target.x < transform.position.x)
        //{
        //    camSpeedX = -2 * Time.deltaTime;
        //}
        //else
        //{
        //    camSpeedX = 0;
        //    Debug.Log("camSpeedX " + camSpeedX);

        //}
        //if (target.y > transform.position.y)
        //{
        //    camSpeedY = 2 * Time.deltaTime;

        //}
        //else if (target.y < transform.position.y)
        //{
        //    camSpeedY = -2 * Time.deltaTime;
        //}
        //else
        //{
        //    camSpeedY = 0;
        //    Debug.Log("camSpeedY " + camSpeedY);
        //}

        //transform.position = new Vector3(transform.position.x + camSpeedX, transform.position.y + camSpeedY, transform.position.z);


        //offsetX += Input.GetAxis("Horizontal") * 0.1f;
        //offsetY += Input.GetAxis("Vertical") * 0.1f;
        //transform.position = new Vector3(linkedObject.transform.position.x + offsetX, linkedObject.transform.position.y + offsetY, -10);




        //if (transform.position.x > target.x)
        //{
        //    transform.position = new Vector3(target.x, transform.position.y, transform.position.z);
        //}
        //if (linkedObject.transform.position.y - transform.position.y > linkedObject.transform.position.y - targetY)
        //{
        //    transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        //}

    }

    void FixedCamera() //camera mode 3
    {

    }

    void CameraTransition(int newMode, Vector2 newPosition)
    {

    }
}
