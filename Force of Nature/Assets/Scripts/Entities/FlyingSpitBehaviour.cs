using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSpitBehaviour : MonoBehaviour
{

    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.instance.PlaySFX("EggCrack");

        if (collision.transform.gameObject.layer == 8)
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

}
