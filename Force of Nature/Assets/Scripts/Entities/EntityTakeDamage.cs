using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EntityTakeDamage : MonoBehaviour
{
    public enum activeEffect
    {
        NONE, ICE, FIRE, WATER, STUN, STEAM
    }

    public activeEffect act;

    public int health;
    private SpriteRenderer spr;
    public EnemyDataScrObj enemyData;
    public PlayerDataScrObj playerData;
    private Rigidbody2D rb;
    private float kbDir;
    private Vector2 kbPower;
    private Vector2 posMod;
    private GameObject player;
    public GameObject dmgTextPrefab;
    private float kbTime;
    private bool teleport;
    private int ability;
    float attackTime = 0.2f;
    float burnTimer;
    float elementTimer;
    float elementTime;
    public float moveSpeedMulti;
    public GameObject hpBar;
    public Animator particle;

    bool fresh;

    public GameObject DeadVFX;

    [SerializeField] private SimpleFlash flashEffect;

    bool waveKb;
    bool ice;
    bool fire;
    bool water;
    public bool frozen;
    void Start()
    {
        burnTimer = 0f;
        moveSpeedMulti = 1f;
        elementTimer = 0f;
        health = enemyData.maxHealth;
        Debug.Log(health + "dummy hp");
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        ice = false;
        fire = false;
        water = false;
        //if(DeadVFX==null)
        //{
        //    DeadVFX=
        //}
        fresh = false;
        waveKb = false;
    }

    IEnumerator FixFireFreeze()
    {
        fresh = true;
        yield return new WaitForSeconds(0.5f);
        fresh = false;
    }
    public void TakeDamage(int dmg, float dir, float atkTime, GameObject playerp, bool playerAtk)
    {
        Debug.Log("hit!");
        AudioManager.instance.HitRandom();
        health -= dmg;
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position, Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.clr = Color.white;
        dtb.dmg = dmg.ToString();
        kbDir = dir;
        kbTime = atkTime;
        player = playerp;
        Debug.Log("time at collision: " + kbTime);
        Debug.Log("start pos" + transform.position.x);
        Debug.Log("remaining hp" + health);
        if (health <= 0)
        {
            Debug.Log("die");
            Die();
        }
        else
        {
            flashEffect.Flash();
            Debug.Log("kb start");
            StartCoroutine("Knockback");
        }
    }

    public void TakeAbilityDamage(int dmg, int id)
    {
        health -= dmg;
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position, Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = dmg.ToString();
        if (this.gameObject.layer == 7)
        {
            switch (id)
            {
                case 1: //icicle
                    dtb.clr = new Color(0.6f, 0.6f, 1, 1);

                    break;
                case 2: //wave
                    dtb.clr = new Color(0.2f, 0.2f, 1, 1);
                    rb.velocity = new Vector2(30 * playerData.faceDir, rb.velocity.y);
                    waveKb = true;
                    StartCoroutine("WaveKB");
                    break;
                case 3: //fire
                    dtb.clr = new Color(1, 0.3f, 0.01f, 1);

                    break;
                default:
                    break;
            }
            if (health <= 0)
            {
                Debug.Log("die");
                Die();
            }
            else
            {
                ability = id;
                StartCoroutine("EffectAbility");
            }
        }

    }


    IEnumerator WaveKB()
    {
        waveKb = true;
        yield return new WaitForSeconds(0.3f);
        waveKb = false;
        rb.velocity = Vector2.zero;
    }
    

    void Update()
    {
        if ((int)act > 0) //checks if an effect is active
        {
            elementTimer += Time.deltaTime;
            if (elementTimer > elementTime)
            {
                switch (act)
                {
                    case activeEffect.NONE:
                        break;
                    case activeEffect.ICE:
                        //reset movement speed
                        moveSpeedMulti = 1f;
                        if (frozen)
                        {
                            frozen = false;
                        }
                        break;
                    case activeEffect.FIRE:
                        Debug.Log("stop burning");
                        break;
                    case activeEffect.WATER:
                        break;
                    default:
                        break;
                }
                act = activeEffect.NONE;
                particle.SetInteger("activeEffect", (int)act);
                spr.color = Color.white;
                //check for active effect
                //if there is one, set it to NONE
            }
            else
            {
                if (act == activeEffect.FIRE)
                {
                    burnTimer += Time.deltaTime;
                    if (burnTimer >= 0.7f)
                    {
                        int damag = Random.Range(45, 56);
                        health -= damag;
                        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position, Quaternion.identity);
                        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
                        dtb.dmg = damag.ToString();
                        dtb.clr = new Color(1, 0.3f, 0.01f, 1);
                        Debug.Log("burning, " + health);
                        if (health <= 0)
                        {
                            Debug.Log("die");
                            Die();
                        }
                        burnTimer = 0;

                    }

                }
                //only for certain effects, like fire, do the damage ticks here
            }
        }

    }
    IEnumerator EffectAbility()
    {
        switch (ability)
        {
            case 1: //icicle
                ElementEffects(1);
                break;
            case 2: //wave
                ElementEffects(2);
                break;
            case 3: //fire
                ElementEffects(3);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0.01f);
    }

    private void ElementEffects(int element)
    {
        elementTimer = 0f;
        switch (element)
        {
            case 1: //cold
                if (act == activeEffect.WATER)
                {
                    ApplyFreeze();
                    //ice on water
                }
                else if (act == activeEffect.FIRE)
                {
                    //ice on fire
                    ApplyIceFire();
                }
                else
                {
                    ApplyCold();
                    //ice normal effect
                }
                break;
            case 2: //water
                if (act == activeEffect.ICE)
                {
                    ApplyStun();
                    //water on ice
                }
                else if (act == activeEffect.FIRE)
                {
                    //water on fire
                    ApplyExtinguish();
                }
                else
                {
                    ApplyWet();
                    //water
                }
                break;
            case 3: //fire
                if (act == activeEffect.WATER)
                {
                    ApplySteam();
                }
                else if (act == activeEffect.ICE)
                {
                    ApplyMelt();
                }
                else
                {
                    ApplyBurning();
                }
                break;

            default:
                break;
        }
    }

    private void ApplyWet()
    {
        act = activeEffect.WATER;
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = "Wet";
        dtb.clr = new Color(0.2f, 0.2f, 1, 1);
        //spr.color = Color.blue;
        elementTime = 4f;
        particle.SetInteger("activeEffect", (int)act);

    }

    private void ApplyBurning()
    {
        act = activeEffect.FIRE;
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = "Burning";
        dtb.clr = new Color(1, 0.3f, 0.01f, 1);
        //spr.color = Color.red;
        elementTime = 4f;
        particle.SetInteger("activeEffect", (int)act);

    }

    private void ApplyCold()
    {
        act = activeEffect.ICE;
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = "Cold";
        dtb.clr = new Color(0.6f, 0.6f, 1, 1);
        //spr.color = Color.cyan;
        elementTime = 4f;
        moveSpeedMulti = 0.4f;
        particle.SetInteger("activeEffect", (int)act);

    }

    private void ApplyStun()
    {
        //water on ice
        //create stun state
        //apply it here
        //set element effect to none
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = "Stun";
        dtb.clr = new Color(0.8f, 0.8f, 0.8f, 1);
        act = activeEffect.STUN;
        elementTime = 3f;
        //spr.color = new Color(0.8f, 0.8f, 0.8f, 1);
        particle.SetInteger("activeEffect", (int)act);

    }

    private void ApplyIceFire()
    {
        //deal extra damage
        //set state to wet
        int damag = Random.Range(700, 801);
        health -= damag;
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = damag.ToString();
        dtb.clr = new Color(0.2f, 0.2f, 1, 1);
        GameObject dText = Instantiate(dmgTextPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        DamageTextBehaviour dt = dText.GetComponent<DamageTextBehaviour>();
        dt.dmg = "Melted";
        dt.clr = new Color(0.2f, 0.2f, 1, 1);
        act = activeEffect.WATER;
        StartCoroutine("FixFireFreeze");
        //spr.color = Color.blue;
        elementTime = 2f;
        particle.SetInteger("activeEffect", (int)act);

    }

    private void ApplyFreeze()
    {
        //apply ice
        if (!fresh)
        {
         act = activeEffect.ICE;
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = "Frozen";
        dtb.clr = new Color(0f, 0.5775638f, 1, 1);
        //spr.color = new Color(0f, 0.5775638f, 1, 1);
        frozen = true;
        elementTime = 8f;
        particle.SetInteger("activeEffect", (int)act);
        }
       

    }

    private void ApplySteam()
    {
        //launch
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = "Steamed";
        dtb.clr = Color.gray;
        act = activeEffect.STEAM;
        elementTime = 0.5f;
        //spr.color = Color.white;
        particle.SetInteger("activeEffect", (int)act);
        rb.velocity = new Vector2(rb.velocity.x, 40);
    }

    private void ApplyExtinguish()
    {
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = "Extinguished";
        dtb.clr = Color.blue;
        act = activeEffect.WATER;
        playerData.health += 25;
        hpBar.GetComponent<Slider>().value = playerData.health;
        //spr.color = Color.blue;
        elementTime = 2f;
        particle.SetInteger("activeEffect", (int)act);

    }

    private void ApplyMelt()
    {
        act = activeEffect.NONE;
        int damag = Random.Range(800, 801);
        health -= damag;
        GameObject dmgText = Instantiate(dmgTextPrefab, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        DamageTextBehaviour dtb = dmgText.GetComponent<DamageTextBehaviour>();
        dtb.dmg = damag.ToString();
        dtb.clr = new Color(1, 0.3f, 0.01f, 1);
        GameObject dText = Instantiate(dmgTextPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        DamageTextBehaviour dt = dText.GetComponent<DamageTextBehaviour>();
        dt.dmg = "Thawed";
        dt.clr = new Color(1, 0.3f, 0.01f, 1);
        act = activeEffect.WATER;
        //spr.color = Color.white;
        particle.SetInteger("activeEffect", (int)act);

        //makes enemy take extra damage
    }

    IEnumerator Knockback()
    {
        if (gameObject.layer != 10 && gameObject.GetComponent<FlyingMonsterBehaviour>() == null)
        {
            switch (playerData.atkType)
            {
                case PlayerDataScrObj.AttackType.MELEE_NOBOOST:
                    teleport = false;
                    Debug.Log("hit no boost");
                    kbTime = 0.001f;
                    rb.velocity = new Vector2(0, enemyData.knockUp);
                    transform.position = new Vector2(transform.position.x, transform.position.y + 0.3f);
                    posMod = new Vector2(3f * kbDir, 3f);
                    break;
                case PlayerDataScrObj.AttackType.MELEE_NOBOOSTAIR:
                    teleport = false;
                    kbTime = 0.001f;
                    Debug.Log("hit no boost air");
                    posMod = new Vector2(3f * kbDir, 3f);
                    break;
                case PlayerDataScrObj.AttackType.MELEE_FORWARDBOOST:
                    teleport = true;
                    Debug.Log("hit fwd boost");
                    kbTime = playerData.atkTimeForwardGround - kbTime;
                    transform.position = new Vector2(transform.position.x, transform.position.y + 0.3f);
                    kbPower = (new Vector2(kbDir, rb.velocity.y) * playerData.atkPower_ForwardGround) + new Vector2(0, 100);
                    posMod = new Vector2(3f * kbDir, 3f);
                    break;
                case PlayerDataScrObj.AttackType.MELEE_FORWARDAIRBOOST:
                    teleport = true;
                    Debug.Log("hit fwd air boost");
                    kbTime = playerData.atkTimeForwardAir - kbTime;
                    kbPower = new Vector2(kbDir, 1) * playerData.atkPower_ForwardAir;
                    posMod = new Vector2(1.5f * kbDir, 0.2f);
                    break;
                case PlayerDataScrObj.AttackType.MELEE_UPBOOST:
                    teleport = true;
                    Debug.Log("hit up");
                    kbTime = playerData.atkTimeUp - kbTime;
                    kbPower = playerData.atkPower_Up;
                    posMod = new Vector2(0.3f * kbDir, 0.7f);
                    break;
                case PlayerDataScrObj.AttackType.MELEE_UPLEFTBOOST:
                    teleport = true;
                    Debug.Log("hit up left");
                    kbTime = playerData.atkTimeUp - kbTime;
                    kbPower = playerData.atkPower_UpLeft;
                    posMod = new Vector2(-1, 1) * 1.2f;
                    break;
                case PlayerDataScrObj.AttackType.MELEE_UPRIGHTBOOST:
                    teleport = true;
                    Debug.Log("hit up right");
                    kbTime = playerData.atkTimeUp - kbTime;
                    kbPower = playerData.atkPower_UpRight;
                    posMod = Vector2.one * 1.2f;
                    break;
                default:
                    break;
            }
        }


        //switch (playerData.atkType)
        //{
        //    case EnemyDataScrObj.DamageType.MELEE_NOBOOST:
        //        Debug.Log("HIT");

        //        rb.velocity = new Vector2(rb.velocity.x, enemyData.knockUp);
        //        break;
        //    case EnemyDataScrObj.DamageType.MELEE_FORWARDBOOST:
        //        Debug.Log("HIT FWD");
        //        rb.velocity = new Vector2(playerData.atkForwardKnockback, enemyData.knockUp);
        //        break;
        //    case EnemyDataScrObj.DamageType.MELEE_BACKBOOST:
        //        Debug.Log("HIT BACK");
        //        rb.velocity = new Vector2(-playerData.atkForwardKnockback, enemyData.knockUp);
        //        break;
        //    case EnemyDataScrObj.DamageType.MELEE_UPBOOST:
        //        Debug.Log("HIT UP");

        //        break;
        //    case EnemyDataScrObj.DamageType.MELEE_UPLEFTBOOST:
        //        Debug.Log("HIT UP LEFT");

        //        break;
        //    case EnemyDataScrObj.DamageType.MELEE_UPRIGHTBOOST:
        //        Debug.Log("HIT UP RIGHT");

        //        break;
        //    default:
        //        break;
        //}
        Debug.Log("time remaining: " + kbTime);
        rb.velocity = kbPower;
        float kbTimer = 0f;
        while (kbTimer < kbTime)
        {
            yield return new WaitForFixedUpdate();
            Debug.Log(rb.velocity.x);
            rb.velocity = kbPower;
            Debug.Log(rb.velocity.x);
            kbTimer += Time.deltaTime;
        }
        rb.velocity = Vector2.zero;
        Debug.Log(transform.position.y + " end pos");
        Debug.Log("player pos " + player.transform.position.x + " " + player.transform.position.y);
        if (teleport)
        {
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y) + posMod;
        }
    }

    public void Die()
    {
        if (gameObject.layer == 7)
        {
            playerData.levelKills += 1;
        }
        gameObject.SetActive(false);
        Instantiate(DeadVFX, this.transform.position, Quaternion.identity);
    }
    // Start is called before the first frame update


    // Update is called once per frame

}
