using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    public bool flip;
    private LayerMask ground;
    private BoxCollider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        flip = false;
        ground = LayerMask.GetMask("Platform");
        coll = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        flip = GroundCheck();
    }

    private bool GroundCheck()
    {
        if (Physics2D.BoxCast(gameObject.transform.position, coll.bounds.size, 0f, Vector2.down, 0.2f, ground))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
