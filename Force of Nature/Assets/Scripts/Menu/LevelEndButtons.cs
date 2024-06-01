using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class LevelEndButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnBacktomenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnContinue()
    {
        SceneManager.LoadScene(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
