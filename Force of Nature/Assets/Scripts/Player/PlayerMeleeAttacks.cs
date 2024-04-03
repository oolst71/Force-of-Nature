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

    private void Start()

    {
        timer = 0;
        pC = GetComponent<PlayerController>();
        velReset = false;
        attackActive = false;
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
                if (playerData.sideAttackBoosted)
                {
                    Debug.Log("attack " + pC.faceDir);
                    if (pC.grounded)
                    {
                        yield return new WaitForSeconds(0.03f);
                    }
                    else
                    {
                        playerData.currentState = PlayerDataScrObj.playerState.ATTACKINGUNLOCKED;
                        yield return new WaitForSeconds(0.05f);
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
                        yield return new WaitForSeconds(0.03f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.05f);
                    }

                }

                break;
            case -1: //attack down
                if (pC.grounded)
                {
                    Debug.Log("attack down");
                }
                else
                {
                    Debug.Log("attack down air");
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

                }
                switch (eightDirAim.x)
                {
                    case -1:
                        Debug.Log("attack up left");
                        break;
                    case 0:
                        Debug.Log("attack up straight");
                        break;
                    case 1:
                        Debug.Log("attack up right");
                        break;
                    default:
                        break;
                }
                yield return new WaitForSeconds(0.05f);
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
        yield return new WaitForSeconds(0.2f);
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
}
