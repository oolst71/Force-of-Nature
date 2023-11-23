using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject linkedObject; //the object that the camera is following
    public Rigidbody2D objectRb;
    public int cameraMode; //the mode of the camera - 1: following an object, 2: locked to an object (1 but without adjusting its position based on momentum), 3: locked onto a certain point (can be useful for, like, boss fights)
   
    [SerializeField] private float camSpeed;


    public float xOffset;
    public float xMomentum;
    public float xProgress;
    public float xCurrentTarget;
    public float xTarget;
    public float xSource;
    public float xDistance;
    public float xCurrentDistance;
    public float xMaxOffset;

    Vector2 boundsTopLeft; //these two variables together form a rectangle that works as the bounds of the camera - regardless of where the player goes, the camera will never move outside of these bounds
    Vector2 boundsBottomRight;




    private void Start()
    {
        xMomentum = 2f;
        cameraMode = 1;
        xTarget = 0;
        xProgress = 1;
        xCurrentTarget = 0;
        xMaxOffset = 2.5f;
        objectRb = linkedObject.GetComponent<Rigidbody2D>();
    }


    private void LateUpdate() //using lateupdate so the camera's position updates after the linked object's
    {
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
        //camera starts out on the player
        //on the X axis, camera always wants to be in front of the player, either +3 or -3

        //when the xTarget changes, set xSource and calculate xDistance

        if (objectRb.velocity.x != 0)
        {
            xTarget = Mathf.Sign(objectRb.velocity.x) * 3;
        }
        if (xTarget != xCurrentTarget)
        {
            xCurrentTarget = xTarget;
            xSource = xOffset;
            xDistance = xCurrentTarget - xSource;
            xProgress = 0;
        }
        if (xProgress < 1)
            xCurrentDistance = Mathf.Abs(xCurrentTarget - xOffset);
        {
            if (xProgress > 0.7f)
            {
                xMomentum = 1.5f;
                if (xProgress > 0.85f)
                {
                    xMomentum = 1f;
                    if (xProgress > 0.95f)
                    {
                        xMomentum = 0.7f;
                    }
                }
            }
            else
            {
                xMomentum = 2f;
            }
        xProgress += xMomentum * Time.deltaTime;
        }
        xOffset = xSource + (xProgress * xDistance);
        if (Mathf.Abs(xOffset) > 3)
        {
            xOffset = 3 * Mathf.Sign(xOffset);
        }
        transform.position = new Vector3(linkedObject.transform.position.x + xOffset, linkedObject.transform.position.y, -10);
    }

    void FixedCamera() //camera mode 3
    {

    }

    void CameraTransition(int newMode, Vector2 newPosition)
    {

    }
}
