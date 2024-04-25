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
    public GameObject waveHeadPrefab;
    public Transform iciclePoint;
    public bool abCooldown;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
                    StartCoroutine("WaterAbility");
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
        rb.velocity = Vector2.zero;
        playerData.currentState = PlayerDataScrObj.playerState.CASTING;
        yield return new WaitForSeconds(0.1f); //windup
        //execute ability here
        //instantiate wave
        Instantiate(waveHeadPrefab, new Vector2(transform.position.x + playerData.faceDir, transform.position.y - 0.5f), Quaternion.identity);
        yield return new WaitForSeconds(0.1f); //recovery
        playerData.currentState = PlayerDataScrObj.playerState.IDLE;
        yield return new WaitForSeconds(playerData.abilityCd);
        abCooldown = true;
    }

    private IEnumerator IceAbility()
    {
        abCooldown = false;
        playerData.currentState = PlayerDataScrObj.playerState.CASTING;
        yield return new WaitForSeconds(0.3f); //windup
        Instantiate(iciclePrefab, new Vector2(iciclePoint.parent.transform.position.x + playerData.faceDir, iciclePoint.position.y), Quaternion.identity);
        yield return new WaitForSeconds(0.3f); //recovery
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
