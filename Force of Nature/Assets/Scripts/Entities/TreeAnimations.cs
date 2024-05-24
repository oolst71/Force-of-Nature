using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimations : MonoBehaviour
{

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimateAttack(int i)
    {
        switch (i)
        {
            case 1:
                anim.SetTrigger("StartAtk");
                //anim.ResetTrigger("Attack1");
                break;
            case 2:
                anim.SetTrigger("Swing");
                //anim.ResetTrigger("Attack2");

                break;
            case 3:
                anim.SetTrigger("EndAtk");
                //anim.ResetTrigger("EndAtk");

                break;
            case 4:
                anim.SetTrigger("Hurt");
                break;
            case 5:
                anim.SetTrigger("Recovered");
                break;
            default:
                break;
        }
    }
}
