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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        } else if (collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<EntityTakeDamage>().TakeAbilityDamage(playerData.icicleDmg, 1);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2 (speed * dirX, 0);
    }
}
