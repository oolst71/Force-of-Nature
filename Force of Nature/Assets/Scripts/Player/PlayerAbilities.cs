using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{

    [SerializeField] private PlayerDataScrObj playerData;
    private SpriteRenderer spr;
    private PlayerDataScrObj.eqElement eq;
    public GameObject iciclePrefab;
    public Transform iciclePoint;
    public bool abCooldown;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.color = Color.white;
        abCooldown = true;
    }

    private void OnAbility()
    {
        Debug.Log("break");
        if (playerData.abilitiesUnlocked && abCooldown)
        {
            switch (eq)
            {
                case PlayerDataScrObj.eqElement.BLIZZARD:
                    //ice ability
                    Debug.Log("ice ability");
                    StartCoroutine("IceAbility");
                    break;
                case PlayerDataScrObj.eqElement.WILDFIRE:
                    Debug.Log("fire ability");
                    //fire ability
                    break;
                case PlayerDataScrObj.eqElement.TSUNAMI:
                    Debug.Log("water ability");
                    //water ability
                    break;
                default:
                    break;
            }
        }
        
    }


     private void OnSwapAbilityLeft()
        {
        if (playerData.abilitiesUnlocked)
        {
            playerData.equipped--;
            if (playerData.equipped < 0)
            {
                playerData.equipped = playerData.loadout.Length - 1;
            }
            eq = playerData.loadout[playerData.equipped];
            SetColor();

        }

    }
    private void OnSwapAbilityRight()
        {

        if (playerData.abilitiesUnlocked)
        {
             playerData.equipped++;
                if (playerData.equipped >= playerData.loadout.Length)
                {
                    playerData.equipped = 0;
                }
            eq = playerData.loadout[playerData.equipped];

            SetColor();
        }
    }


    private IEnumerator WaterAbility()
    {
        abCooldown = false;
        playerData.currentState = PlayerDataScrObj.playerState.CASTING;
        yield return new WaitForSeconds(0.1f); //windup
        //execute ability here
        yield return new WaitForSeconds(0.1f); //recovery
        playerData.currentState = PlayerDataScrObj.playerState.IDLE;
        yield return new WaitForSeconds(playerData.abilityCd);
        abCooldown = true;
    }

    private IEnumerator IceAbility()
    {
        abCooldown = false;
        playerData.currentState = PlayerDataScrObj.playerState.CASTING;
        yield return new WaitForSeconds(0.1f); //windup
        Instantiate(iciclePrefab, new Vector2(iciclePoint.parent.transform.position.x + playerData.faceDir, iciclePoint.position.y), Quaternion.identity);
        yield return new WaitForSeconds(0.2f); //recovery
        playerData.currentState = PlayerDataScrObj.playerState.IDLE;
        yield return new WaitForSeconds(playerData.abilityCd);
        abCooldown = true;
    }

    public void SetColor()
    {
        switch (eq)
        {
            case PlayerDataScrObj.eqElement.BLIZZARD:
                spr.color = Color.cyan;

                break;
            case PlayerDataScrObj.eqElement.WILDFIRE:
                spr.color = Color.red;

                break;
            case PlayerDataScrObj.eqElement.TSUNAMI:
                spr.color = Color.blue;

                break;
            default:
                break;
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
