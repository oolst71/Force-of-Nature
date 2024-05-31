using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAbilityBehaviour : MonoBehaviour
{

    [SerializeField] private PlayerDataScrObj playerData;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void FlameActivate()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void FlameDeactivate()
    {
        GetComponent<BoxCollider2D>().enabled = false;

    }

    public void FlameDie()
    {
        Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<EntityTakeDamage>().TakeAbilityDamage(playerData.fireDmg, 3);
        }
    }
}
