using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAbilityBehaviour : MonoBehaviour
{

    [SerializeField] private PlayerDataScrObj playerData;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FireBehaviour");
    }

    IEnumerator FireBehaviour()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<EntityTakeDamage>().TakeAbilityDamage(playerData.fireDmg, 3);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
