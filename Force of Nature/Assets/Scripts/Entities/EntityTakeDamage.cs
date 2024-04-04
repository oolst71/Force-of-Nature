using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTakeDamage : MonoBehaviour
{
    private int health;
    public EnemyDataScrObj enemyData;
    private Rigidbody2D rb;
    void Start()
    {
        health = enemyData.maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int dmg,  EnemyDataScrObj.DamageType type)
    {
        Debug.Log("hit!");
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            switch (type)
            {
                case EnemyDataScrObj.DamageType.MELEE_NOBOOST:
                    //pop up slightly into the air
                    rb.velocity = new Vector2(rb.velocity.x, enemyData.knockUp);
                    break;
                case EnemyDataScrObj.DamageType.MELEE_FORWARDBOOST:
                    break;
                case EnemyDataScrObj.DamageType.MELEE_BACKBOOST:
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
        }


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
