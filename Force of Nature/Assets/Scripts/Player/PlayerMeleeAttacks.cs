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
    private float atkDashTimer;


    public bool attackDash;

    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public GameObject upleft;
    public GameObject upright;
    private GameObject selected;

    private RaycastHit2D[] hits;
    LayerMask hittable;

    //delete this garbage later
    private bool testing;
    private float logPos;

    private void Start()

    {
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
        if (attackDash)
        {
            atkDashTimer += Time.deltaTime;
        }
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
                    if (pC.grounded)
                    {
                        yield return new WaitForSeconds(playerData.atkTimeForwardUnmoving / 2);
                        Debug.Log("halfway velocity: " + rb.velocity.x);
                        yield return new WaitForSeconds(playerData.atkTimeForwardUnmoving / 2);
                    }
                    else
                    {
                        playerData.currentState = PlayerDataScrObj.playerState.ATTACKINGUNLOCKED;
                        yield return new WaitForSeconds(playerData.atkTimeForwardUnmoving / 2);
                        Debug.Log("halfway velocity: " + rb.velocity.x);
                        yield return new WaitForSeconds(playerData.atkTimeForwardUnmoving / 2);
                    }
                    hits = Physics2D.CircleCastAll(transform.position, playerData.atkSizeForward, Vector2.right * pC.faceDir, 1f, hittable);
                    foreach (RaycastHit2D hit in hits)
                    {
                        hit.transform.gameObject.GetComponent<EntityTakeDamage>().TakeDamage(1, EnemyDataScrObj.DamageType.MELEE_NOBOOST, playerData.atkTimeForwardUnmoving);
                    }
                }
                else
                {
                    playerData.sideAttackBoosted = true;
                    velReset = true;
                    if (pC.grounded)
                    {
                        hits = Physics2D.CircleCastAll(new Vector2(transform.position.x + pC.faceDir, transform.position.y), playerData.atkSizeForward, Vector2.right * pC.faceDir, 1f, hittable);
                        rb.velocity = new Vector2(playerData.sideAttackPower * pC.faceDir, 0f);
                        Debug.Log("ground atk");
                        Debug.Log("velocity at start of attack is " + rb.velocity.x);
                        yield return new WaitForSeconds(playerData.atkTimeForwardGround / 2);
                        Debug.Log("halfway velocity " + rb.velocity.x);
                        yield return new WaitForSeconds(playerData.atkTimeForwardGround / 2);
                    }
                    else
                    {
                        hits = Physics2D.CircleCastAll(new Vector2(transform.position.x + pC.faceDir, transform.position.y), playerData.atkSizeForward, Vector2.right * pC.faceDir, 2f, hittable);
                        rb.velocity = new Vector2((playerData.sideAttackPower + playerData.airAttackBoost) * pC.faceDir, 0f);
                        Debug.Log("velocity at start of attack is " + rb.velocity.x);
                        yield return new WaitForSeconds(playerData.atkTimeForwardAir); //TODO: Replace all of these with timers adjustable from playerData

                    }
                    if (pC.faceDir > 0)
                    {
                        if (pC.grounded)
                        {
                            foreach (RaycastHit2D hit in hits)
                            {
                                hit.transform.gameObject.GetComponent<EntityTakeDamage>().TakeDamage(1, EnemyDataScrObj.DamageType.MELEE_FORWARDBOOST, playerData.atkTimeForwardGround);
                            }
                        }
                        else
                        {
                            foreach (RaycastHit2D hit in hits)
                            {
                                hit.transform.gameObject.GetComponent<EntityTakeDamage>().TakeDamage(1, EnemyDataScrObj.DamageType.MELEE_FORWARDBOOST, playerData.atkTimeForwardAir);
                            }

                        }

                    }
                    else
                    {
                        if (pC.grounded)
                        {
                            foreach (RaycastHit2D hit in hits)
                            {
                                hit.transform.gameObject.GetComponent<EntityTakeDamage>().TakeDamage(1, EnemyDataScrObj.DamageType.MELEE_BACKBOOST, playerData.atkTimeForwardGround);
                            }
                        }
                        else
                        {
                            foreach (RaycastHit2D hit in hits)
                            {
                                hit.transform.gameObject.GetComponent<EntityTakeDamage>().TakeDamage(1, EnemyDataScrObj.DamageType.MELEE_BACKBOOST, playerData.atkTimeForwardAir);
                            }

                        }
                    }

                }
               

                break;
            case -1: //attack down
                selected = down;
                selected.GetComponent<SpriteRenderer>().enabled = true;

                if (pC.grounded)
                {
                    yield return new WaitForSeconds(playerData.atkTimeDownAir);
                }
                else
                {
                    playerData.currentState = PlayerDataScrObj.playerState.ATTACKINGUNLOCKED;
                    yield return new WaitForSeconds(playerData.atkTimeDownAir);
                }
                break;
            case 1: //attack up
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
                    rb.velocity = new Vector2(0f,playerData.upAttackPower);
                switch (eightDirAim.x)
                    {
                        case -1:
                            selected = upleft;
                            rb.velocity = new Vector2(-playerData.sideAttackPower / 2, playerData.upAttackPower * 0.8f);
                            break;
                        case 0:
                            selected = up;
                            rb.velocity = new Vector2(0f, playerData.upAttackPower);
                            break;
                        case 1:
                            selected = upright;
                            rb.velocity = new Vector2(playerData.sideAttackPower / 2, playerData.upAttackPower * 0.8f);
                            break;
                        default:
                            break;
                    }
                }
                selected.GetComponent<SpriteRenderer>().enabled = true;

                yield return new WaitForSeconds(playerData.atkTimeUp);
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
        Gizmos.DrawSphere(new Vector2(transform.position.x + pC.faceDir, transform.position.y),playerData.atkSizeForward);
    }
}
