using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXCleaner : MonoBehaviour
{
    private void Start()
    {
        Invoke("DestroyVFX", 5);
    }
    private void DestroyVFX()
    {
        Destroy(this.gameObject);
    }
}
