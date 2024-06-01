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

    public void AnimDeath(bool dead)
    {
        if (dead)
        {
            anim.SetTrigger("Die");
        }
        else
        {
            anim.SetTrigger("Respawn");
        }
    }

    public void ChangeAnimState(int st)
    {
        anim.SetInteger("currState", st);
        //Debug.Log("setting state" + count);
        count++;
    }

    public void AnimGround(bool st, float yvel)
    {
        anim.SetBool("Grounded", st);
        anim.SetFloat("yVel", yvel);
    }

    public void AnimDash(int i)
    {
        switch (i)
        {
            case 1:
                anim.SetTrigger("UpDash");
                break;
            case 2:
                anim.SetTrigger("FwdDash");
                break;
            case 3:
                anim.SetTrigger("DownDash");
                break;
            case 4:
                anim.SetTrigger("EndDash");
                break;
            default:
                break;
        }
    }

    public void ResetTriggers()
    {
        anim.ResetTrigger("Attack1");
        anim.ResetTrigger("Attack2");
        anim.ResetTrigger("EndAtk");
        anim.ResetTrigger("AirAtk");
    }

    public void AnimJump(int i)
    {
        switch (i)
        {
            case 1:
                anim.SetTrigger("Jump");
                Debug.Log("jump trigger");
                break;
            default:
                break;
        }
    }

    public void AnimateAbility(int i)
    {
        switch (i)
        {
            case 1:
                anim.SetTrigger("WaterAbility");
                break;
            case 2:
                anim.SetTrigger("FireAbility");
                break;
            case 3:
                anim.SetTrigger("IceAbility");
                break;
            case 4:
                anim.SetTrigger("EndAbility");
                break;
            default:
                break;
        }
    }

    public void AnimateAttack(int i)
    {
        ResetTriggers();
        switch (i)
        {
            case 1:
                anim.SetTrigger("Attack1");
                //anim.ResetTrigger("Attack1");
                break;
            case 2:
                anim.SetTrigger("Attack2");
                //anim.ResetTrigger("Attack2");

                break;
            case 3:
                anim.SetTrigger("EndAtk");
                //anim.ResetTrigger("EndAtk");

                break;
            case 4:
                anim.SetTrigger("AirAtk");
                break;
            default:
                break;
        }
    }
}
