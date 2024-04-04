using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTakeDamage : MonoBehaviour
{
    private int health;
    public EnemyDataScrObj enemyData;
    public PlayerDataScrObj playerData;
    private Rigidbody2D rb;
    private EnemyDataScrObj.DamageType damageType;
    float attackTime = 0.2f;
    void Start()
    {
        health = enemyData.maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int dmg,  EnemyDataScrObj.DamageType type, float atkTime)
    {
        damageType = type;
        attackTime = atkTime;
        Debug.Log("hit!");
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine("Knockback");
        }


    }


    IEnumerator Knockback()
    {
        switch (damageType)
        {
            case EnemyDataScrObj.DamageType.MELEE_NOBOOST:
                rb.velocity = new Vector2(rb.velocity.x, enemyData.knockUp);
                break;
            case EnemyDataScrObj.DamageType.MELEE_FORWARDBOOST:
                Debug.Log(transform.position.x + " start pos");
                rb.velocity = new Vector2(playerData.atkForwardKnockback, enemyData.knockUp);
                break;
            case EnemyDataScrObj.DamageType.MELEE_BACKBOOST:
                Debug.Log(transform.position.x + " start pos");
                rb.velocity = new Vector2(-playerData.atkForwardKnockback, enemyData.knockUp);
                break;
            case EnemyDataScrObj.DamageType.MELEE_UPBOOST:
                break;
            case EnemyDataScrObj.DamageType.MELEE_UPLEFTBOOST:
                break;
            case EnemyDataScrObj.DamageType.MELEE_UPRIGHTBOOST:
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(attackTime + playerData.atkRecoveryTime);

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
