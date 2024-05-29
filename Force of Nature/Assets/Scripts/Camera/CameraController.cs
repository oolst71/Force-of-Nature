using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject linkedObject; //the object that the camera is following
    public Rigidbody2D objectRb;
    public int cameraMode; //the mode of the camera - 1: following an object, 2: locked to an object (1 but without adjusting its position based on momentum), 3: locked onto a certain point (can be useful for, like, boss fights)
   
    [SerializeField] private float camSpeed;
    [SerializeField]private GameObject upperLimit;
    [SerializeField] private GameObject lowerLimit;

    public float xOffset;
    public float xMomentum;
    public float xProgress;
    public float xCurrentTarget;
    public float xTarget;
    public float xSource;
    public float xDistance;
    public float xCurrentDistance;
    public float xMaxOffset;

    public float yBaseOffset;
    public float yOffset;
    public float yMomentum;
    public float yProgress;
    public float yCurrentTarget;
    public float yTarget;
    public float ySource;
    public float yDistance;
    public float yCurrentDistance;
    public float yMaxOffset;
    private float baseY;

    Vector2 boundsTopLeft; //these two variables together form a rectangle that works as the bounds of the camera - regardless of where the player goes, the camera will never move outside of these bounds
    Vector2 boundsBottomRight;




    private void Start()
    {
        xOffset = 0;
        yOffset = 7.5f;
        xMomentum = 2f;
        yMomentum = 1.5f;
        xTarget = 0;
        xProgress = 1;
        xCurrentTarget = 0;
        yCurrentTarget = 0;
        xMaxOffset = 2.5f;
        yBaseOffset = 3;
        yMaxOffset = -5;
        objectRb = linkedObject.GetComponent<Rigidbody2D>();
        baseY = linkedObject.transform.position.y;
    }


    private void LateUpdate() 
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

    void FollowingCamera() //camera mode 2, just follows the object
    {
        if (linkedObject.transform.position.y > baseY + yOffset)
        {
            if (linkedObject.transform.position.y > upperLimit.transform.position.y)
            {
                transform.position = new Vector3(linkedObject.transform.position.x + xOffset, upperLimit.transform.position.y, -10);

            }
            else
            {
                transform.position = new Vector3(linkedObject.transform.position.x + xOffset, linkedObject.transform.position.y, -10);

            }
        }
        else
        {
            transform.position = new Vector3(linkedObject.transform.position.x + xOffset, lowerLimit.transform.position.y, -10);

        }
    }

    void LooseFollowingCamera() //camera mode 1
    {
        //camera starts out on the player
        //on the X axis, camera always wants to be in front of the player, either +3 or -3

        //when the xTarget changes, set xSource and calculate xDistance


        //x axis camera movement
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
        if (xProgress < 1) { 
            xCurrentDistance = Mathf.Abs(xCurrentTarget - xOffset);
        
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




        //y axis camera movement
        //if (objectRb.velocity.y < 0f)
        //{
        //    yTarget = -5;
        //}
        //else
        //{
        //    yTarget = 0;
        //}
        //if (yTarget != yCurrentTarget)
        //{
        //    yCurrentTarget = yTarget;
        //    ySource = yOffset;
        //    yDistance = yCurrentTarget - ySource;
        //    yProgress = 0;
        //}
        //if (yProgress < 1) { 
        //    yCurrentDistance = Mathf.Abs(yCurrentTarget - yOffset);
        
        //    if (yProgress > 0.7f)
        //    {
        //        yMomentum = 2f;
        //        if (yProgress > 0.85f)
        //        {
        //            yMomentum = 1.5f;
        //            if (yProgress > 0.95f)
        //            {
        //                yMomentum = 1f;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        yMomentum = 2.5f;
        //    }
        //    yProgress += yMomentum * Time.deltaTime;
        //}
        //yOffset = ySource + (yProgress * yDistance);
        //if (yOffset < -5)
        //{
        //    yOffset = -5;
        //} else if (yOffset > 0)
        //{
        //    yOffset = 0;
        //}
        if (linkedObject.transform.position.y > upperLimit.transform.position.y)
        {
            transform.position = new Vector3(linkedObject.transform.position.x + xOffset, upperLimit.transform.position.y, -10);

        }
        else if (linkedObject.transform.position.y < lowerLimit.transform.position.y)
        {
            transform.position = new Vector3(linkedObject.transform.position.x + xOffset, lowerLimit.transform.position.y, -10);

        }
        else
        {
            transform.position = new Vector3(linkedObject.transform.position.x + xOffset, linkedObject.transform.position.y, -10);
        }
    }

    void FixedCamera() //camera mode 3
    {

    }

    void CameraTransition(int newMode, Vector2 newPosition)
    {

    }
}
