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
    private Vector2 retainedMomentum;
    private float timer;

    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public GameObject upleft;
    public GameObject upright;
    private GameObject selected;

    private RaycastHit2D[] hits;
    LayerMask hittable;

    private void Start()

    {
        timer = 0;
        pC = GetComponent<PlayerController>();
        velReset = false;
        attackActive = false;
        hittable = LayerMask.GetMask("Enemies");
    }

    private void Update()
    {
        timer += Time.deltaTime;
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
        retainedMomentum = rb.velocity;
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
                    Debug.Log("attack " + pC.faceDir);
                    if (pC.grounded)
                    {
                        yield return new WaitForSeconds(0.05f);
                    }
                    else
                    {
                        playerData.currentState = PlayerDataScrObj.playerState.ATTACKINGUNLOCKED;
                        yield return new WaitForSeconds(0.08f);
                    }
                    hits = Physics2D.CircleCastAll(transform.position, playerData.atkSizeForward, Vector2.right * pC.faceDir, 1f, hittable);
                    foreach (RaycastHit2D hit in hits)
                    {
                        hit.transform.gameObject.GetComponent<EntityTakeDamage>().TakeDamage(1, EnemyDataScrObj.DamageType.MELEE_NOBOOST);
                    }
                }
                else
                {
                    Debug.Log("attack boosted " + pC.faceDir);
                    playerData.sideAttackBoosted = true;
                    velReset = true;
                    rb.velocity = new Vector2(playerData.sideAttackPower * pC.faceDir, 0f);
                    if (pC.grounded)
                    {
                        rb.velocity = new Vector2(playerData.sideAttackPower * pC.faceDir, 0f);
                        yield return new WaitForSeconds(0.05f);
                        hits = Physics2D.CircleCastAll(transform.position, playerData.atkSizeForward, Vector2.right * pC.faceDir, 1f, hittable);

                    }
                    else
                    {
                        rb.velocity = new Vector2((playerData.sideAttackPower + playerData.airAttackBoost) * pC.faceDir, 0f);
                        yield return new WaitForSeconds(0.08f); //TODO: Replace all of these with timers adjustable from playerData
                        hits = Physics2D.CircleCastAll(transform.position, playerData.atkSizeForward, Vector2.right * pC.faceDir, 1f, hittable);

                    }
                    if (pC.faceDir > 0)
                    {
                        foreach (RaycastHit2D hit in hits)
                        {
                            hit.transform.gameObject.GetComponent<EntityTakeDamage>().TakeDamage(1, EnemyDataScrObj.DamageType.MELEE_FORWARDBOOST);
                        }
                    }
                    else
                    {
                        foreach (RaycastHit2D hit in hits)
                        {
                            hit.transform.gameObject.GetComponent<EntityTakeDamage>().TakeDamage(1, EnemyDataScrObj.DamageType.MELEE_BACKBOOST);
                        }
                    }

                }
               

                break;
            case -1: //attack down
                selected = down;
                selected.GetComponent<SpriteRenderer>().enabled = true;

                if (pC.grounded)
                {
                    Debug.Log("attack down");
                    yield return new WaitForSeconds(0.08f);
                }
                else
                {
                    Debug.Log("attack down air");
                    playerData.currentState = PlayerDataScrObj.playerState.ATTACKINGUNLOCKED;
                    yield return new WaitForSeconds(0.08f);
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
                    Debug.Log("boosted: ");
                    playerData.upAttackBoosted = true;
                    velReset = true;
                    rb.velocity = new Vector2(0f,playerData.upAttackPower);
                switch (eightDirAim.x)
                    {
                        case -1:
                            Debug.Log("attack up left");
                            selected = upleft;
                            rb.velocity = new Vector2(-playerData.sideAttackPower / 2, playerData.upAttackPower * 0.8f);
                            break;
                        case 0:
                            Debug.Log("attack up straight");
                            selected = up;
                            rb.velocity = new Vector2(0f, playerData.upAttackPower);
                            break;
                        case 1:
                            selected = upright;
                            Debug.Log("attack up right");
                            rb.velocity = new Vector2(playerData.sideAttackPower / 2, playerData.upAttackPower * 0.8f);
                            break;
                        default:
                            break;
                    }
                }
                
                selected.GetComponent<SpriteRenderer>().enabled = true;

                yield return new WaitForSeconds(0.08f);
                break;
            default:
                break;

        }

        if (velReset)
        {
            rb.velocity = Vector2.zero;
            velReset = false;
        } else if (pC.grounded)
        {
            if (Mathf.Abs(pC.aim.x) > playerData.deadzoneX)
            {
                playerData.currAccel = playerData.maxWalkSpeed * pC.faceDir;
            };
        }
        yield return new WaitForSeconds(0.15f);
        selected.GetComponent<SpriteRenderer>().enabled = false;
        if (!attackQueued)
        {
            Debug.Log("exiting attack " + timer);
            attackActive = false;
            pC.ResetState();
        }
        else
        {
            Debug.Log("starting queued attack! " + timer);
            StartCoroutine("MeleeAttack");
        }

    }

    IEnumerator QueueAttack()
    {
        if (attackQueued == false)
        {
            Debug.Log("queuing attack!");
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
