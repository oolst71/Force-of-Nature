using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextBehaviour : MonoBehaviour
{

    private TMP_Text text;
    private Rigidbody2D rb;
    public string dmg;
    public Color clr;
    private float lifeTimer;
    private float op;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("summon");
        text = GetComponent<TMP_Text>();
        rb = GetComponent<Rigidbody2D>();
        text.text = dmg;
        text.color = clr;
        op = 1;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(0, 1f);

        op -= 0.04f;
        if (op < 0)
        {
            Destroy(gameObject);
        }
        text.color = new Color(clr.r, clr.g, clr.b, op);
    }

    // Update is called once per frame

}
