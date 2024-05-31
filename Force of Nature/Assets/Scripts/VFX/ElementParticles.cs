using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementParticles : MonoBehaviour
{
    

    public void EnableSprite()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void DisableSprite()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

}
