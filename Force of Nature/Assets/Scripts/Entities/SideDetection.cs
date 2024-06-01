using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDetection : MonoBehaviour
{

    public bool flip;
    // Start is called before the first frame update
    void Start()
    {
        flip = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7)
        {
            flip = true;
        }
    }
}
