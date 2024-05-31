using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FwdHitbox : MonoBehaviour
{
    // Start is called before the first frame update

    private BoxCollider2D coll;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        coll.offset = new Vector2(-1.257054f, coll.offset.y);
    }
}
