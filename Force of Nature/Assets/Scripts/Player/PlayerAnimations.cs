using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private PlayerDataScrObj playerData;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        anim.SetBool("grounded", playerData.gd);
    }

    // Update is called once per frame

    public void ChangeAnimState(int st)
    {

    }

}
