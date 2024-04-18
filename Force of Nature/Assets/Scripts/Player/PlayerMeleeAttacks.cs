using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMeleeAttacks : MonoBehaviour
{
    [SerializeField]private PlayerController pC;
    [SerializeField] private PlayerDataScrObj playerData;
    [SerializeField] private Rigidbody2D rb;
    private bool velReset;
    private bool attackActive;
    private bool attackQueued;
    private Vector2 atkQueueDir;
    private float timer;
    private float logTimer;
    private float atkDashTime;
    public float atkDashTimer;


    public bool attackDash;

    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public GameObject upleft;
    public GameObject upright;
    private GameObject selected;

    [SerializeField]private GameObject fwdBox;
    [SerializeField] private GameObject upBox;
    [SerializeField] private GameObject downBox;
    [SerializeField] private GameObject downAirBox;
    private GameObject activeBox;

    private RaycastHit2D[] hits;
    LayerMask hittable;

    //delete this garbage later
    private bool testing;
    private float logPos;

    private void Start()

    {
        fwdBox.SetActive(false);
        upBox.SetActive(false);
        downBox.SetActive(false);
        downAirBox.SetActive(false);
        timer = 0;
        pC = GetComponent<PlayerController>();
        velReset = false;
        attackActive = false;
        hittable = LayerMask.GetMask("Enemies");
        attackDash = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {

    }

    private void OnAttack()
    {
        if (playerData.playerStates[(int)playerData.currentState].canAttack)
        {
            playerData.currentState = PlayerDataScrObj.playerState.ATTACKING;
            StartCoroutine("MeleeAttack");
        }
        
        else if (playerData.playerStates[(int)playerData.currentState].canQueueAttacks)
        {
            StartCoroutine("QueueAttack");
        }
    }

    IEnumerator MeleeAttack()
    {
        atkDashTimer = 0f;
        logTimer = timer;
        logPos = transform.position.x;
        attackActive = true;
        Vector2 eightDirAim;
        if (attackQueued)
        {
            eightDirAim = atkQueueDir;
            attackQueued = false;
        }
        else
        {
            eightDirAim = pC.aim;
        }
        if (Mathf.Abs(pC.aim.y) >= playerData.deadzoneY)
        {
            eightDirAim.y = Mathf.Sign(pC.aim.y);
        }
        else
        {
            eightDirAim.y = 0;
        }

        switch (eightDirAim.y)
        {
            case 0: //attack forward
                activeBox = fwdBox;
                activeBox.SetActive(true);
                if (pC.faceDir < 0)
                {
                    selected = left;
                }
                else
                {
                    selected = right;
                }
                selected.GetComponent<SpriteRenderer>().enabled = true;
                if (playerData.sideAttackBoosted)
                {
                    playerData.atkType = PlayerDataScrObj.AttackType.MELEE_NOBOOST;
                    if (pC.grounded)
                    {
                        while (atkDashTimer < playerData.atkTimeForwardUnmoving)
                        {
                            yield return new WaitForFixedUpdate();
                            atkDashTimer += Time.deltaTime;
                        }
                    }
                    else
                    {
                        playerData.atkType = PlayerDataScrObj.AttackType.MELEE_NOBOOSTAIR;
                        playerData.currentState = PlayerDataScrObj.playerState.ATTACKINGUNLOCKED;
                        while (atkDashTimer < playerData.atkTimeForwardUnmoving)
                        {
                            yield return new WaitForFixedUpdate();
                            atkDashTimer += Time.deltaTime;
                        }
                    }

                }
                else
                {
                    playerData.sideAttackBoosted = true;
                    velReset = true;
                    if (pC.grounded)
                    {
                        playerData.atkType = PlayerDataScrObj.AttackType.MELEE_FORWARDBOOST;
                        rb.velocity = new Vector2(pC.faceDir, 1) * playerData.atkPower_ForwardGround;
                        Debug.Log("ground atk");
                        Debug.Log("velocity at start of attack is " + rb.velocity.x);
                        while (atkDashTimer < playerData.atkTimeForwardGround)
                        {
                            yield return new WaitForFixedUpdate();
                            Debug.Log("player: " + rb.velocity.x);
                            rb.velocity = new Vector2(pC.faceDir, 1) * playerData.atkPower_ForwardGround;
                            atkDashTimer += Time.deltaTime;
                        }

                    }
                    else
                    {
                        playerData.atkType = PlayerDataScrObj.AttackType.MELEE_FORWARDAIRBOOST;
                        rb.velocity = new Vector2(pC.faceDir, 1) * playerData.atkPower_ForwardAir;

                        Debug.Log("velocity at start of attack is " + rb.velocity.x);
                        while (atkDashTimer < playerData.atkTimeForwardAir)
                        {
                            yield return new WaitForFixedUpdate();
                        rb.velocity = new Vector2(pC.faceDir, 1) * playerData.atkPower_ForwardAir;
                            atkDashTimer += Time.deltaTime;
                        }

                    }
                }
               

                break;
            case -1: //attack down
                selected = down;
                selected.GetComponent<SpriteRenderer>().enabled = true;
                if (pC.grounded)
                {
                    rb.velocity = Vector2.zero;
                    playerData.atkType = PlayerDataScrObj.AttackType.MELEE_NOBOOST;
                    activeBox = downBox;
                    activeBox.SetActive(true);
                    while (atkDashTimer < playerData.atkTimeDownGround)
                    {
                        yield return new WaitForFixedUpdate();
                        atkDashTimer += Time.deltaTime;
                    }
                }
                else
                {
                    playerData.atkType = PlayerDataScrObj.AttackType.MELEE_NOBOOSTAIR;
                    activeBox = downAirBox;
                    activeBox.SetActive(true);
                    playerData.currentState = PlayerDataScrObj.playerState.ATTACKINGUNLOCKED;
                    yield return new WaitForSeconds(playerData.atkTimeDownAir);
                    while (atkDashTimer < playerData.atkTimeDownAir)
                    {
                        yield return new WaitForFixedUpdate();
                        atkDashTimer += Time.deltaTime;
                    }
                }
                break;
            case 1: //attack up - MAY NEED TO ADD GRAVITY HERE IN THE FUTURE
                activeBox = upBox;
                activeBox.SetActive(true);
                if (Mathf.Abs(pC.aim.x) >= playerData.deadzoneX)
                {
                    eightDirAim.x = Mathf.Sign(pC.aim.x);
                }
                else
                {
                    eightDirAim.x = 0;
                }
                if (!playerData.upAttackBoosted)
                {
                    playerData.upAttackBoosted = true;
                    velReset = true;
                switch (eightDirAim.x)
                    {
                        case -1:
                            selected = upleft;
                            playerData.atkType = PlayerDataScrObj.AttackType.MELEE_UPLEFTBOOST;
                            selected.GetComponent<SpriteRenderer>().enabled = true;

                            rb.velocity = playerData.atkPower_UpLeft;
                            while (atkDashTimer < playerData.atkTimeUp)
                            {
                                yield return new WaitForFixedUpdate();
                                rb.velocity = playerData.atkPower_UpLeft;
                                atkDashTimer += Time.deltaTime;
                            }
                            break;
                        case 0:
                            selected = up;
                            playerData.atkType = PlayerDataScrObj.AttackType.MELEE_UPBOOST;
                            selected.GetComponent<SpriteRenderer>().enabled = true;

                            rb.velocity = playerData.atkPower_Up;
                            while (atkDashTimer < playerData.atkTimeUp)
                            {
                                yield return new WaitForFixedUpdate();
                                rb.velocity = playerData.atkPower_Up;
                                atkDashTimer += Time.deltaTime;
                            }
                            break;
                        case 1:
                            selected = upright;
                            playerData.atkType = PlayerDataScrObj.AttackType.MELEE_UPRIGHTBOOST;
                            selected.GetComponent<SpriteRenderer>().enabled = true;

                            rb.velocity = playerData.atkPower_UpRight;
                            while (atkDashTimer < playerData.atkTimeUp)
                            {
                                yield return new WaitForFixedUpdate();
                                rb.velocity = playerData.atkPower_UpRight;
                                atkDashTimer += Time.deltaTime;
                            }
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                break;

        }

        Debug.Log("switch passed at: " + (timer - logTimer));

        if (velReset)
        {
            Debug.Log("velocity reset at: " + (timer - logTimer));
            Debug.Log("velocity pre-reset is" + rb.velocity.x);
            rb.velocity = Vector2.zero;
            velReset = false;
        } else if (pC.grounded)
        {
            if (Mathf.Abs(pC.aim.x) > playerData.deadzoneX)
            {
                playerData.currAccel = playerData.maxWalkSpeed * pC.faceDir;
            };
        }
        activeBox.SetActive(false);
        yield return new WaitForSeconds(playerData.atkRecoveryTime);
        selected.GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log(transform.position.x - logPos + " POS: " + transform.position.x + "atk complete at: " + (timer - logTimer));
        if (!attackQueued)
        {
            attackActive = false;
            pC.ResetState();
        }
        else
        {
            StartCoroutine("MeleeAttack");
        }

    }

    IEnumerator QueueAttack()
    {
        if (attackQueued == false)
        {
            yield return new WaitForSeconds(0.03f);
            attackQueued = true;
            atkQueueDir = pC.aim;
        }
        else
        {
            yield return null;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + pC.faceDir, transform.position.y),playerData.atkSizeForward);
    }
}