using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyDeadVFX : MonoBehaviour
{
    public GameObject FlyingEnemyPuddle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == 6)
        {
            Instantiate(FlyingEnemyPuddle, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}
