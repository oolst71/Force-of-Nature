using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSpitBehaviour : MonoBehaviour
{

    [SerializeField] int damage;
    private Animator anim;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();    
        rb = GetComponent<Rigidbody2D>();
    }


    public void EggBreak()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.transform.gameObject.layer == 8)
        {
            AudioManager.instance.PlaySFX("EggCrack");
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.TakeDamage(15);
        }
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetTrigger("Break");
    }

}
