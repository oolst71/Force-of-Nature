using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IcicleBehaviour : MonoBehaviour
{

    [SerializeField] private float speed;
    public float dirX;
    [SerializeField] private PlayerDataScrObj playerData;
    private Rigidbody2D rb;
    [SerializeField] private List<Sprite> icicleSprites;
    private SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = icicleSprites[Random.Range(0, icicleSprites.Count)];
        rb = GetComponent<Rigidbody2D>();
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
