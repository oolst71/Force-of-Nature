using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IcicleBehaviour : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float dirX;
    [SerializeField] private PlayerDataScrObj playerData;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dirX = playerData.faceDir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //AudioManager.instance.PlaySFX("Ice");

        if (collision.gameObject.layer == 6)
        {
            AudioManager.instance.PlaySFX("Ice");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 7 || collision.gameObject.layer == 10)
        {
            AudioManager.instance.PlaySFX("Ice");
            collision.gameObject.GetComponent<EntityTakeDamage>().TakeAbilityDamage(playerData.icicleDmg, 1);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2 (speed * dirX, 0);
    }
}
