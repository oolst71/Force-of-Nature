using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public Animation testAnim;
    // Start is called before the first frame update
    void Start()
    {
        testAnim.Play("Player_Run");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
