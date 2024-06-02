using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAbilities : MonoBehaviour
{

    [SerializeField] private PlayerDataScrObj playerData;
    [SerializeField] private SpriteRenderer spr;
    private PlayerDataScrObj.eqElement eq;
    public GameObject iciclePrefab;
    public GameObject waveHeadPrefab;
    public GameObject firePrefab;
    public Transform iciclePoint;
    public Transform flamePoint;
    public bool abCooldown;
    private Rigidbody2D rb;
    private float newAbilityCd;
    private float faceDirectionDuringCast;
    [SerializeField] private PlayerAnimations anims;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr.color = Color.white;
        abCooldown = true;
        newAbilityCd = playerData.abilityCd;
        playerData.equipped = 0;
        eq = PlayerDataScrObj.eqElement.BLIZZARD;
    }

    private void OnAbility()
    {

        Debug.Log("break");
        if ((playerData.abilitiesUnlocked || SceneManager.GetActiveScene().buildIndex == 5) && abCooldown && playerData.gd)
        {
            rb.velocity = Vector3.zero;
            switch (eq)
            {
                case PlayerDataScrObj.eqElement.BLIZZARD:
                    //ice ability
                    Debug.Log("ice ability");
                    StartCoroutine("IceAbility");
                    break;
                case PlayerDataScrObj.eqElement.WILDFIRE:
                    Debug.Log("fire ability");
                    StartCoroutine("FireAbility");
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
        newAbilityCd = playerData.abilityCd -= .15f;

        Debug.Log("eq " + eq);
        Debug.Log("equipped " + playerData.equipped);
        Debug.Log("loadout 0" + playerData.loadout[0]);
        Debug.Log("loadout 1" + playerData.loadout[1]);
        Debug.Log("loadout 2" + playerData.loadout[2]);

        if (playerData.abilitiesUnlocked || SceneManager.GetActiveScene().buildIndex == 5)
        {
            playerData.equipped--;
            if (playerData.equipped < 0)
            {
                playerData.equipped = playerData.loadout.Length - 1;
            }
            Debug.Log("new eq " + eq);
            Debug.Log("new equipped " + playerData.equipped);
            eq = playerData.loadout[playerData.equipped];
            Debug.Log("new new eq " + eq);
            Debug.Log("new new equipped " + playerData.equipped);

            SetColor();

        }

    }
    private void OnSwapAbilityRight()
    {
        newAbilityCd = playerData.abilityCd -= .15f;

        if (playerData.abilitiesUnlocked || SceneManager.GetActiveScene().buildIndex == 5)
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
        anims.AnimateAbility(1);

        faceDirectionDuringCast = playerData.faceDir;
        yield return new WaitForSeconds(0.1f); //windup
        //execute ability here
        //instantiate wave
        if (playerData.faceDir > 0)
        {
            AudioManager.instance.WaterAttackRandom();
            AudioManager.instance.PlaySFX("Tsunami");
            Instantiate(waveHeadPrefab, new Vector2(transform.position.x + faceDirectionDuringCast * 2f, transform.position.y + 0.2f), Quaternion.Euler(new Vector3(0, 180, 0)));

        }
        else
        {
            AudioManager.instance.WaterAttackRandom();

            AudioManager.instance.PlaySFX("Tsunami");
            Instantiate(waveHeadPrefab, new Vector2(transform.position.x + faceDirectionDuringCast * 2f, transform.position.y + 0.2f), Quaternion.Euler(new Vector3(0, 0, 0)));

        }
        yield return new WaitForSeconds(0.33f); //recovery
        if (playerData.faceDir != faceDirectionDuringCast)
        {
            playerData.faceDir = faceDirectionDuringCast;
        }

        playerData.currentState = PlayerDataScrObj.playerState.IDLE;
        anims.AnimateAbility(4);
        yield return new WaitForSeconds(newAbilityCd);
        newAbilityCd = playerData.abilityCd;
        abCooldown = true;
    }

    private IEnumerator IceAbility()
    {
        abCooldown = false;
        playerData.currentState = PlayerDataScrObj.playerState.CASTING;
        anims.AnimateAbility(3);

        faceDirectionDuringCast = playerData.faceDir;
        yield return new WaitForSeconds(0.2f); //windup
        AudioManager.instance.IceAttackRandom();

        GameObject icicle1 = Instantiate(iciclePrefab, new Vector2(iciclePoint.parent.transform.position.x + (faceDirectionDuringCast * 2.4f), iciclePoint.position.y + 1f), Quaternion.identity);
        GameObject icicle2 = Instantiate(iciclePrefab, new Vector2(iciclePoint.parent.transform.position.x + (faceDirectionDuringCast * 2.2f), iciclePoint.position.y), Quaternion.identity);
        GameObject icicle3 = Instantiate(iciclePrefab, new Vector2(iciclePoint.parent.transform.position.x + faceDirectionDuringCast, iciclePoint.position.y + 1.5f), Quaternion.identity);
        GameObject icicle4 = Instantiate(iciclePrefab, new Vector2(iciclePoint.parent.transform.position.x + (faceDirectionDuringCast * 1.3f), iciclePoint.position.y - 0.5f), Quaternion.identity);
        GameObject icicle5 = Instantiate(iciclePrefab, new Vector2(iciclePoint.parent.transform.position.x + (faceDirectionDuringCast * 1.6f), iciclePoint.position.y + 0.5f), Quaternion.identity);

        icicle1.GetComponent<IcicleBehaviour>().dirX = faceDirectionDuringCast;
        icicle2.GetComponent<IcicleBehaviour>().dirX = faceDirectionDuringCast;
        icicle3.GetComponent<IcicleBehaviour>().dirX = faceDirectionDuringCast;
        icicle4.GetComponent<IcicleBehaviour>().dirX = faceDirectionDuringCast;
        icicle5.GetComponent<IcicleBehaviour>().dirX = faceDirectionDuringCast;

        yield return new WaitForSeconds(0.2f); //recovery
        if (playerData.faceDir != faceDirectionDuringCast)
        {
            playerData.faceDir = faceDirectionDuringCast;
        }

        anims.AnimateAbility(4);
        playerData.currentState = PlayerDataScrObj.playerState.IDLE;
        yield return new WaitForSeconds(newAbilityCd);
        newAbilityCd = playerData.abilityCd;
        abCooldown = true;
    }

    private IEnumerator FireAbility()
    {
        abCooldown = false;
        playerData.currentState = PlayerDataScrObj.playerState.CASTING;
        anims.AnimateAbility(2);
        
        faceDirectionDuringCast = playerData.faceDir;
        yield return new WaitForSeconds(0.2f); //windup 
        AudioManager.instance.FireAttackRandom();
        AudioManager.instance.PlaySFX("Fire");
        if (faceDirectionDuringCast > 0)
        {
        Instantiate(firePrefab, new Vector2(transform.position.x + faceDirectionDuringCast * 1.25f, transform.position.y + 1.4f), Quaternion.Euler(new Vector3(0, 180, 0)));
        }
        else
        {
        Instantiate(firePrefab, new Vector2(transform.position.x + faceDirectionDuringCast * 1.25f, transform.position.y + 1.4f), Quaternion.Euler(new Vector3(0, 0, 0)));

        }

        yield return new WaitForSeconds(0.2f); //recovery
        if (playerData.faceDir != faceDirectionDuringCast)
        {
            playerData.faceDir = faceDirectionDuringCast;
        }
        anims.AnimateAbility(4);
        playerData.currentState = PlayerDataScrObj.playerState.IDLE;
        yield return new WaitForSeconds(newAbilityCd);
        newAbilityCd = playerData.abilityCd;
        abCooldown = true;
    }

    public void SetColor()
    {
        switch (eq)
        {
            case PlayerDataScrObj.eqElement.BLIZZARD:
                //spr.color = Color.cyan;
                UIManager.Instance.SetBlizzardUI();

                break;
            case PlayerDataScrObj.eqElement.WILDFIRE:
                //spr.color = Color.red;
                UIManager.Instance.SetWildfireUI();
                break;
            case PlayerDataScrObj.eqElement.TSUNAMI:
                //spr.color = Color.blue;
                UIManager.Instance.SetTsunamiUI();
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
