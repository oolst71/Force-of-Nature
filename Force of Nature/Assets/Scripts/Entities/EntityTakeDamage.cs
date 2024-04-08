using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTakeDamage : MonoBehaviour
{
    private int health;
    public EnemyDataScrObj enemyData;
    public PlayerDataScrObj playerData;
    private Rigidbody2D rb;
    private float kbDir;
    private Vector2 kbPower;
    private float kbTime;
    float attackTime = 0.2f;
    void Start()
    {
        health = enemyData.maxHealth;
        Debug.Log(health + "dummy hp");
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int dmg, float dir, float atkTime)
    {
        Debug.Log("hit!");
        health -= dmg;
        kbDir = dir;
        kbTime = atkTime;
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
            Debug.Log("kb start");
            StartCoroutine("Knockback");
        }
    }


    IEnumerator Knockback()
    {

        switch (playerData.atkType)
        {
            case PlayerDataScrObj.AttackType.MELEE_NOBOOST:
                Debug.Log("hit no boost");
                kbTime = 0.001f;
                rb.velocity = new Vector2(0, enemyData.knockUp);
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
                break;
            case PlayerDataScrObj.AttackType.MELEE_NOBOOSTAIR:
                kbTime = 0.001f;
                Debug.Log("hit no boost air");
                break;
            case PlayerDataScrObj.AttackType.MELEE_FORWARDBOOST:
                Debug.Log("hit fwd boost");
                kbTime = playerData.atkTimeForwardGround;
               kbPower = new Vector2(kbDir, 1) * playerData.atkPower_ForwardGround;
                break;
            case PlayerDataScrObj.AttackType.MELEE_FORWARDAIRBOOST:
                Debug.Log("hit fwd air boost");
                kbTime = playerData.atkTimeForwardAir;
                kbPower = new Vector2(kbDir, 1) * playerData.atkPower_ForwardAir;
                break;
            case PlayerDataScrObj.AttackType.MELEE_UPBOOST:
                Debug.Log("hit up");
                //kbTime = playerData.atkTimeUp - kbTime;
                kbPower = playerData.atkPower_Up;
                break;
            case PlayerDataScrObj.AttackType.MELEE_UPLEFTBOOST:
                Debug.Log("hit up left");
                //kbTime = playerData.atkTimeUp - kbTime;
                kbPower = playerData.atkPower_UpLeft;
                break;
            case PlayerDataScrObj.AttackType.MELEE_UPRIGHTBOOST:
                Debug.Log("hit up right");
                //kbTime = playerData.atkTimeUp - kbTime;
                kbPower = playerData.atkPower_UpRight;
                break;
            default:
                break;
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
        while(kbTimer < kbTime)
        {
            yield return new WaitForFixedUpdate();
            Debug.Log(rb.velocity.x);
            rb.velocity = kbPower;
            Debug.Log(rb.velocity.x);
            kbTimer += Time.deltaTime;
        }
        rb.velocity = Vector2.zero;
        Debug.Log(transform.position.x + " end pos");

    }

    public void Die()
    {

    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
