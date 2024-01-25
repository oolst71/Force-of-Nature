using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 3f;
    [SerializeField] float dashPower = 50f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 0.5f;
    [SerializeField] float coyoteTime = 0.1f;

    private Rigidbody2D rb;
    private CapsuleCollider2D coll;

    private float baseGravity;
    private float movementInput; //the horizontal axis input of the player
    private Vector2 aim; //the stick input of the player
    private Vector2 dashDir;
    private float faceDir; //the direction the player is facing

    private LayerMask ground;
    [SerializeField]private bool grounded;
    private bool dashCd;
    private bool dashing;

    private float coyoteTimer;
    private float dashCooldownTimer;
    private float dashDurationTimer;

    //ok so these will need to be reworked whenever i figure out a better way to do this, because there's gotta be a better way to do this
    private bool canMove; //set this to false to make the player completely unable to move by themselves (for cutscenes, for example)
    private bool canWalk; //set this to false to make the player unable to walk/run
    private bool canJump; //set this to false to make the player unable to jump, even if they're grounded
    private bool canDash; //set this to false to make the player unable to dash, even if the dash cooldown is up

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        ground = LayerMask.GetMask("Platform"); //here you put in any layers that the player can step on
        canMove = true;
        canWalk = true;
        canJump = true;
        canDash = true;
        dashCd = true;
        dashing = false;
        baseGravity = rb.gravityScale;
        coyoteTimer = 0f;
        dashCooldownTimer = 100f;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(aim.x * moveSpeed, rb.velocity.y);
        }

        grounded = GroundCheck();

        if (!grounded)
        {
            coyoteTimer += Time.deltaTime;
        }
        else
        {
            coyoteTimer = 0;
        }

        if (dashing)
        {
            ProcessDash();
        }
        else
        {
            dashCooldownTimer += Time.deltaTime;
        }

    }

    private void OnJump()
    {
        if (canMove && canJump && (grounded || coyoteTimer <= coyoteTime))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private void OnStickInput(InputValue value)
    {
        aim = value.Get<Vector2>();
        if (aim.x != 0)
        {
            faceDir = Mathf.Sign(aim.x);
        }
    }

   private void OnTestAxis(InputValue value)
    {
        movementInput = value.Get<float>();
    }

    private void OnDash()
    {
        if (canDash && canMove && dashCooldownTimer > dashCooldown)
        {
            dashDir = aim;
            Debug.Log("aiming " + dashDir.x + " " + dashDir.y);
            if (aim.y < 0.35f && aim.y > -0.35f)
            {
                dashDir.y = 0f;
                if (aim.x == 0f)
                {
                    dashDir.x = faceDir;
                }
            }
            Debug.Log("dashing " + dashDir.x + " " + dashDir.y);
            dashDir.Normalize();
            StartDash();
        }
    }

    private bool GroundCheck()
    {
        //boxcast directly downwards
        //if it hits the "ground" layermask (which contains anything the player can stand on), it counts as being grounded
        if (Physics2D.BoxCast(gameObject.transform.position, 0.95f * coll.bounds.size, 0f, Vector2.down, 0.2f, ground))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StartDash()
    {
        canMove = false;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashDir.x * dashPower, dashDir.y * dashPower);
        dashing = true;
        dashDurationTimer = 0f;
    }

    private void ProcessDash()
    {
        if (dashDurationTimer > dashDuration) //end dash
        {
            dashCooldownTimer = 0f;
            rb.velocity = Vector2.zero;
            rb.gravityScale = baseGravity;
            canMove = true;
            dashing = false;
            rb.velocity = Vector2.zero;
            dashCooldownTimer = 0f;
        } else
        {
            dashDurationTimer += Time.deltaTime;
        }
    }
}
