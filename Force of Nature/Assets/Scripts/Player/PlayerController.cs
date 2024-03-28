using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private PlayerDataScrObj playerData;

    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private TrailRenderer trail;

    private Vector2 aim; //the stick input of the player
    private Vector2 dashDir; 
    private float faceDir; 

    private LayerMask ground;
    [SerializeField]private bool grounded;
    private float coyoteTimer;

    [SerializeField]private bool jumpBuffer;

    private bool canMove; //set this to false to make the player completely unable to move by themselves (for cutscenes, for example)
    private bool canWalk; //set this to false to make the player unable to walk/run
    private bool canJump; //set this to false to make the player unable to jump, even if they're grounded
    private bool canDash; //set this to false to make the player unable to dash, even if the dash cooldown is up

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
        ground = LayerMask.GetMask("Platform"); //here you put in any layers that the player can step on
        canMove = true;
        canWalk = true;
        canJump = true;
        canDash = true;
        rb.gravityScale = playerData.baseGravity;
        coyoteTimer = 0f;
    }

    void FixedUpdate()
    {
        if (canMove)
        {

            if (playerData.accelBasedMovement == false)
            {
                rb.velocity = new Vector2(Mathf.Sign(aim.x) * playerData.moveSpeed, rb.velocity.y);
            }
            else
            {
                if (Mathf.Abs(aim.x) > playerData.deadzoneX)
                {
                    playerData.currAccel += playerData.accel;
                    if (playerData.currAccel > playerData.maxWalkSpeed)
                    {
                        playerData.currAccel = playerData.maxWalkSpeed;
                    }
                } else
                {
                    playerData.currAccel = 0f;
                }
               
                rb.velocity = new Vector2(Mathf.Sign(aim.x) * playerData.currAccel, rb.velocity.y);
            }
            CheckMovementBuffers();
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

      

    }

    

    private void OnJump()
    {
        if (canMove && canJump)
        {
            if (grounded || coyoteTimer <= playerData.coyoteTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, playerData.jumpSpeed);
            }
            else
            {
                StartCoroutine("BufferJump");
            }
        }
    }

    private void CheckMovementBuffers()
    {
        if (jumpBuffer && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerData.jumpSpeed);
            jumpBuffer = false;
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

    private void OnDash()
    {
        if (canDash && canMove)
        {
            if (playerData.freeDirectionDash)
            {
                dashDir = aim;
                if (aim.y < 0.35f && aim.y > -0.35f)
                {
                    dashDir.y = 0f;
                    if (aim.x == 0f)
                    {
                        dashDir.x = faceDir;
                    }
                }
            }
            else
            {
                Vector2 eightDirAim = aim;
                if (Mathf.Abs(aim.x) >= playerData.deadzoneX)
                {
                    eightDirAim.x = 1 * Mathf.Sign(aim.x);
                }
                else
                {
                    eightDirAim.x = 0;
                }
                if (Mathf.Abs(aim.y) >= playerData.deadzoneY)
                {
                    eightDirAim.y = 1 * Mathf.Sign(aim.y);
                }
                else
                {
                    eightDirAim.y = 0;
                }
                dashDir = eightDirAim;
            }
            dashDir.Normalize();
            StartCoroutine("PlayerDash");
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

    IEnumerator PlayerDash()
    {
        trail.emitting = true;
        canMove = false;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashDir.x * playerData.dashPower, dashDir.y * playerData.dashPower);

        yield return new WaitForSeconds(playerData.dashDuration);

        rb.velocity = Vector2.zero;
        rb.gravityScale = playerData.baseGravity;
        canMove = true;
        trail.emitting = false;
    }

    IEnumerator BufferJump()
    {
        jumpBuffer = true;
        yield return new WaitForSeconds(playerData.jumpBufferTime);
        jumpBuffer = false;
    }
}
