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
    private bool boostedUp;
    private bool boostedSide;

    private void Start()
    {
        pC = GetComponent<PlayerController>();
        
    }
    private void OnAttack()
    {
        playerData.currentState = PlayerDataScrObj.playerState.ATTACKING;
        StartCoroutine("MeleeAttack");
    }

    IEnumerator MeleeAttack()
    {

        Vector2 eightDirAim = pC.aim;
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
                }
                else
                {
                    Debug.Log("attack boosted " + pC.faceDir);
                    playerData.sideAttackBoosted = true;
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
                break;
            default:
                break;
        }

        //get aim

        //ATTACK TYPES

        //up normal
        //works both on the ground and in the air, the main thing is that it's the first upwards attack in the sequence. once you use it, every following up aerial has no movement
        //until you touch the ground
        //upwards attack with a slight upward or diagonal dash

        //up aerial (no movement)
        //same as up normal except with no movement

        //side normal
        //works both on the ground and in the air. if used in the air, only the first one is normal, the rest have no movement until you touch the ground again.

        //side aerial (no movement)
        //same as side normal, except you don't move

        //down aerial
        //no movement

        //down ground
        //low sweep, no movement

        yield return new WaitForSeconds(0.01f);
        pC.ResetState();
    }
}
