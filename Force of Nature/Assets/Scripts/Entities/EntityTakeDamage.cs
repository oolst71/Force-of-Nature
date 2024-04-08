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
    float attackTime = 0.2f;
    void Start()
    {
        health = enemyData.maxHealth;
        Debug.Log(health + "dummy hp");
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int dmg, float dir)
    {
        Debug.Log("hit!");
        health -= dmg;
        kbDir = dir;
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
                rb.velocity = new Vector2(0, enemyData.knockUp);
                break;
            case PlayerDataScrObj.AttackType.MELEE_FORWARDBOOST:
                Debug.Log("hit fwd boost");
                rb.velocity = new Vector2(playerData.atkForwardKnockback * kbDir , enemyData.knockUp);
                break;
            case PlayerDataScrObj.AttackType.MELEE_FORWARDAIRBOOST:
                Debug.Log("hit fwd air boost");
                rb.velocity = new Vector2(playerData.atkForwardKnockback * 5 * kbDir, enemyData.knockUp);
                break;
            case PlayerDataScrObj.AttackType.MELEE_UPBOOST:
                Debug.Log("hit up");
                rb.velocity = new Vector2(0, 100);
                break;
            case PlayerDataScrObj.AttackType.MELEE_UPLEFTBOOST:
                Debug.Log("hit up left");
                rb.velocity = new Vector2(-20, 20);
                break;
            case PlayerDataScrObj.AttackType.MELEE_UPRIGHTBOOST:
                Debug.Log("hit up right");

                rb.velocity = new Vector2(20, 20);
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
        yield return new WaitForSeconds(playerData.atkRecoveryTime);

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
