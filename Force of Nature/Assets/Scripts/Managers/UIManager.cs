using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //MONKE HEHEHEHEHEHEHEHE
    public static UIManager Instance { get; private set; }

    public GameObject WildfireUI;
    public GameObject BlizzardUI;
    public GameObject TsunamiUI;

    public GameObject WildfireHighlightImage;
    public GameObject BlizzardHighlightImage;
    public GameObject TsunamiHighlightImage;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        WildfireHighlightImage.SetActive(false);
        BlizzardHighlightImage.SetActive(false);
        TsunamiHighlightImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBlizzardUI()
    {
        Debug.Log("blizz called");
        WildfireHighlightImage.SetActive(false);
        BlizzardHighlightImage.SetActive(true);
        TsunamiHighlightImage.SetActive(false);
    }
    public void SetWildfireUI()
    {
        Debug.Log("fire called");

        WildfireHighlightImage.SetActive(true);
        BlizzardHighlightImage.SetActive(false);
        TsunamiHighlightImage.SetActive(false);
    }
    public void SetTsunamiUI()
    {
        Debug.Log("water called");

        WildfireHighlightImage.SetActive(false);
        BlizzardHighlightImage.SetActive(false);
        TsunamiHighlightImage.SetActive(true);
    }
}
