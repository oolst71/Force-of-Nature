using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MeleeHitreg : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerMeleeAttacks attacks;
    private BoxCollider2D coll;
    private float baseOffset;
    [SerializeField] private PlayerDataScrObj playerData;
    [SerializeField] private EnemyDataScrObj.DamageType damageType;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        baseOffset = coll.offset.x;
    }

    private void FixedUpdate()
    {
        coll.offset = new Vector2(playerController.faceDir * baseOffset, coll.offset.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == 7 || collision.transform.gameObject.layer == 10)
        {
            EntityTakeDamage etd = collision.GetComponent<EntityTakeDamage>();
            if (etd != null)
            {
                Debug.Log("got script");
                if (etd.act == EntityTakeDamage.activeEffect.WATER)
                {
                    AudioManager.instance.PlayHeal();
                    playerData.health += 5;
                    //make character blink green
                    if (playerData.health > playerData.maxHealth)
                    {
                        playerData.health = playerData.maxHealth;
                    }
                    playerController.hpBar.GetComponent<Slider>().value = playerData.health;

                }
                etd.TakeDamage(Random.Range(playerData.atkDamage - 25, playerData.atkDamage + 26), playerController.faceDir, attacks.atkDashTimer, transform.parent.gameObject, true);
            }
            else
            {
                Debug.Log("FUUUUUUCK");
            }
        }
    }
}
