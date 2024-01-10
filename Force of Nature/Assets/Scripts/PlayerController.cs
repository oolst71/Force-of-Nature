using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 3f;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private float movementInput; //the horizontal axis input of the player
    private Vector2 aim; //the stick input of the player
    private LayerMask ground;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        ground = LayerMask.GetMask("Platform"); //here you put in any layers that the player can step on
    }

    void Update()
    {
        rb.velocity = new Vector2(aim.x * moveSpeed, rb.velocity.y);
    }

    void OnJump()
    {
        if (GroundCheck())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    void OnStickInput(InputValue value)
    {
        aim = value.Get<Vector2>();
    }

    void OnTestAxis(InputValue value)
    {
        movementInput = value.Get<float>();
    }

    bool GroundCheck()
    {
        //boxcast directly downwards
        //if it hits the "ground" layermask (which contains anything the player can stand on), it counts as being grounded
        if (Physics2D.BoxCast(gameObject.transform.position, 0.95f * coll.bounds.size, 0f, Vector2.down, 0.1f, ground))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
