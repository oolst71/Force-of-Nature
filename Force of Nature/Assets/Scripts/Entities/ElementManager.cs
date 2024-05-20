using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{

    private EntityTakeDamage damageScript;
    bool water;
    bool fire;
    bool ice;

    void Start()
    {
        damageScript = GetComponent<EntityTakeDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
