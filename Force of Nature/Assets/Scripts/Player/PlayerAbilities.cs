using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{

    [SerializeField] private PlayerDataScrObj playerData;
    private SpriteRenderer spr;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.color = Color.white;
    }

    private void OnAbility()
    {
        Debug.Log("break");
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
            SetColor();
        }
    }


    public void SetColor()
    {
        if (playerData.loadout[playerData.equipped] == PlayerDataScrObj.eqElement.BLIZZARD)
        {
            spr.color = Color.cyan;
        }
        else if (playerData.loadout[playerData.equipped] == PlayerDataScrObj.eqElement.WILDFIRE)
        {
            spr.color = Color.red;

        }
        else if (playerData.loadout[playerData.equipped] == PlayerDataScrObj.eqElement.TSUNAMI)
        {
            spr.color = Color.blue;
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
