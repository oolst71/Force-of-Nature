using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingElementBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    private SpriteRenderer spr;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.color = new Color(1, 1, 1, 0);
    }


    public void ActivateElement()
    {
        if (spr.color.a <= 0)
        {
            StartCoroutine("Activ");
        }
    }

    public void DisableElement()
    {
        if (spr.color.a >= 1)
        {
            StartCoroutine("Disab");
        }
    }

    IEnumerator Activ()
    {
        float i = 0;
        while (spr.color.a < 1)
        {
            i += 0.05f;
            spr.color = new Color(1, 1, 1, i);
            yield return new WaitForFixedUpdate();
        }
        spr.color = new Color(1, 1, 1, 1);
    }

    IEnumerator Disab()
    {
        float i = 1;
        while (spr.color.a > 0)
        {
            i -= 0.05f;
            spr.color = new Color(1, 1, 1, i);
            yield return new WaitForFixedUpdate();

        }
        spr.color = new Color(1, 1, 1, 0);
    }
}
