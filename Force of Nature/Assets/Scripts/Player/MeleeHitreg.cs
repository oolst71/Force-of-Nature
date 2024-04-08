using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitreg : MonoBehaviour
{
    [SerializeField]PlayerController playerController;
    [SerializeField]PlayerMeleeAttacks attacks;
    private BoxCollider2D coll;
    private float baseOffset;
    [SerializeField]private PlayerDataScrObj playerData;
    [SerializeField]private EnemyDataScrObj.DamageType damageType;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        baseOffset = coll.offset.x;
    }

    private void FixedUpdate()
    {
        coll.offset = new Vector2(playerController.faceDir * baseOffset,coll.offset.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == 7)
        {
            EntityTakeDamage etd = collision.GetComponent<EntityTakeDamage>();
            if (etd != null)
            {
                Debug.Log("got script");
                etd.TakeDamage(playerData.atkDamage, playerController.faceDir, attacks.atkDashTimer);
            }
            else
            {
                Debug.Log("FUUUUUUCK");
            }
        }
    }
}
