using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private PlayerDataScrObj playerData;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private TrailRenderer trail;
    private SpriteRenderer sprite;
    private PlayerAnimations playerAnim;

    public Vector2 aim; //the stick input of the player
    private Vector2 dashDir;
    public float faceDir;

    private LayerMask ground;
    [SerializeField]public bool grounded;
    private float coyoteTimer;
    private float hurtTimer;
    [SerializeField] private GameObject respawnPoint;
    [SerializeField]private bool jumpBuffer;
    [SerializeField] private GameObject hpBar;

    //For AfterImageEffect
    [Space]
    [Header("After Image FX")]
    [SerializeField] Vector3 _afterImageOffset;
    [SerializeField] private float _distanceBetweenImages;
    private bool _isDashing;
    private float _dashTime;
    private float _dashSpeed;
    private float _dashCooldown;
    private float _dashTimeLeft;
    private float _lastImageXPosition;
    private float _lastDash=-1000f;
    private float _afterImageTimer;
    [SerializeField]
    private float _afterDuration;
    public Transform AfterImageTranform;
    public Vector2 StartDashPosition { get; private set; }

    void Start()
    {
        playerData.health = playerData.maxHealth;
        faceDir = 1;
        playerData.faceDir = 1;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        trail = GetComponent<TrailRenderer>();
        sprite = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<PlayerAnimations>();
        trail.emitting = false;
        ground = LayerMask.GetMask("Platform"); //here you put in any layers that the player can step on
        playerData.currentState = PlayerDataScrObj.playerState.IDLE;
        rb.gravityScale = playerData.baseGravity;
        coyoteTimer = 0f;
        playerData.dashCd = true;
        playerData.airDashed = false;
    }

    void FixedUpdate()
    {
        grounded = GroundCheck();

        if (playerData.currentState == PlayerDataScrObj.playerState.HURT)
        {
            hurtTimer += Time.deltaTime;
            if (hurtTimer > playerData.hurtTime)
            {
                ResetState();
            }
        }
        if (playerData.playerStates[(int)playerData.currentState].canMove)
        {
                if (Mathf.Abs(aim.x) > playerData.deadzoneX)
                {
                    if (faceDir == Mathf.Sign(playerData.currAccel)){
                        playerData.currAccel += playerData.accel * faceDir;
                        if (Mathf.Abs(playerData.currAccel) > playerData.maxWalkSpeed)
                        {
                            playerData.currAccel = faceDir * playerData.maxWalkSpeed;
                        }
                    } else
                    {
                        playerData.currAccel += (playerData.accel + playerData.decel) * faceDir;
                    }
                   
                } else
                {
                    if (playerData.currAccel != 0f)
                    {
                        playerData.currAccel = (Mathf.Abs(playerData.currAccel) - playerData.decel) * faceDir;
                        if (Mathf.Sign(playerData.currAccel) != faceDir)
                        {
                            playerData.currAccel = 0f;
                    }
                }
            }
                rb.velocity = new Vector2(playerData.currAccel, rb.velocity.y);
            if (grounded && rb.velocity.x != 0)
            {
                playerData.currentState = PlayerDataScrObj.playerState.RUNNING;

            }
            else if (rb.velocity == Vector2.zero)
            {
                playerData.currentState = PlayerDataScrObj.playerState.IDLE;
            }
            CheckMovementBuffers();
        }

        if (!grounded)
        {
            coyoteTimer += Time.deltaTime;
        }
        else
        {
            coyoteTimer = 0;
        }
        playerAnim.ChangeAnimState((int)playerData.currentState);

        if (!grounded)
        {
            AfterImagePool.Instance.GetFromPool(AfterImageTranform, sprite, _afterImageOffset);
        }
    }


    public void TakeDamage(int damage)
    {
        playerData.health -= damage;
        hpBar.GetComponent<Slider>().value = playerData.health;
        if (playerData.health <= 0)
        {
            Respawn();
        }
        playerData.currentState = PlayerDataScrObj.playerState.HURT;
        hurtTimer = 0f;
        rb.velocity = Vector2.zero;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.3f);

    }

    public void Respawn()
    {
        transform.position = respawnPoint.transform.position;
        rb.velocity = Vector2.zero;
        ResetState();
        playerData.health = playerData.maxHealth;
        hpBar.GetComponent<Slider>().value = playerData.health;
    }


    private void OnJump()
    {
        if (playerData.playerStates[(int)playerData.currentState].canJump)
        {
            if (grounded || coyoteTimer <= playerData.coyoteTime)
            {
                playerData.currentState = PlayerDataScrObj.playerState.JUMPING;
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
            playerData.currentState = PlayerDataScrObj.playerState.JUMPING;
            rb.velocity = new Vector2(rb.velocity.x, playerData.jumpSpeed);
            jumpBuffer = false;
        }
    }

    private void OnStickInput(InputValue value)
    {
        aim = value.Get<Vector2>();
        if (Mathf.Abs(aim.x) >= playerData.deadzoneX)
        {
            faceDir = Mathf.Sign(aim.x);
            playerData.faceDir = faceDir;
            sprite.transform.localScale = new Vector3(-faceDir, 1, 1);
        }
    }

    private void OnDash()
    {
        if (playerData.playerStates[(int)playerData.currentState].canDash && playerData.dashCd)
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
                    eightDirAim.x = Mathf.Sign(aim.x);
                }
                else
                {
                    eightDirAim.x = 0;
                }
                if (Mathf.Abs(aim.y) >= playerData.deadzoneY)
                {
                    eightDirAim.y = Mathf.Sign(aim.y);
                }
                else
                {
                    eightDirAim.y = 0;
                }
                if (eightDirAim == Vector2.zero)
                {
                    eightDirAim.x = faceDir;
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
        if (Physics2D.BoxCast(gameObject.transform.position, coll.bounds.size, 0f, Vector2.down, 0.2f, ground))
        {
            if (playerData.airDashed)
            {
                playerData.airDashed = false;
                playerData.dashCd = true;
            }
            playerData.sideAttackBoosted = false;
            playerData.upAttackBoosted = false;
            playerData.gd = true;
            return true;
        }
        else
        {
            playerData.gd = false;
            return false;
        }
    }

    IEnumerator PlayerDash()
    {
        if (playerData.airDashed == false || GroundCheck())
        {
            StartCoroutine("DashCooldown");
            //trail.emitting = true;
            StartDashPosition = transform.position;

            
            playerData.currentState = PlayerDataScrObj.playerState.DASHING;
            playerData.dashCd = false;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(dashDir.x * playerData.dashPower, dashDir.y * playerData.dashPower);

            yield return new WaitForSeconds(playerData.dashDuration);

            rb.velocity = Vector2.zero;
            rb.gravityScale = playerData.baseGravity;
            if (GroundCheck() == false)
            {
                playerData.airDashed = true;
            }
            ResetState();
            //trail.emitting = false;
        }
        
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(playerData.dashDuration + playerData.dashCooldown);
        playerData.dashCd = true;
    }

    public void ResetState()
    {
        if (grounded)
        {
            playerData.currentState = PlayerDataScrObj.playerState.IDLE;

        }
        else
        {
            playerData.currentState = PlayerDataScrObj.playerState.JUMPING;
        }
        playerAnim.ChangeAnimState((int)playerData.currentState);
    }
    IEnumerator BufferJump()
    {
        jumpBuffer = true;
        yield return new WaitForSeconds(playerData.jumpBufferTime);
        jumpBuffer = false;
    }
}
