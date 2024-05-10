using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private PlayerDataScrObj playerData;
    int count;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("currState", 4);
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAnimState(int st)
    {
        anim.SetInteger("currState", st);
        Debug.Log("setting state" + count);
        count++;
    }

    public void AnimateAttack(bool sec)
    {
        if (!sec)
        {
            anim.SetTrigger("Attack1");
        }
        else
        {
            anim.SetTrigger("Attack2");
        }
    }
}
