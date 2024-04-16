using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{

    [SerializeField] private PlayerDataScrObj playerData;
    private SpriteRenderer spr;
    private PlayerDataScrObj.eqElement eq;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.color = Color.white;
    }

    private void OnAbility()
    {
        Debug.Log("break");
        if (playerData.abilitiesUnlocked)
        {
            switch (eq)
            {
                case PlayerDataScrObj.eqElement.BLIZZARD:
                    //ice ability
                    Debug.Log("ice ability");
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
