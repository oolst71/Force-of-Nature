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
    public GameObject HealthBar;

    public GameObject WildfireHighlightImage;
    public GameObject BlizzardHighlightImage;
    public GameObject TsunamiHighlightImage;

    public GameObject deathText;
    public GameObject respawn;
    public GameObject returntomenu;
    public GameObject buttons;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        WildfireUI.SetActive(true);
        BlizzardUI.SetActive(true);
        TsunamiUI.SetActive(true);
        WildfireHighlightImage.SetActive(false);
        BlizzardHighlightImage.SetActive(false);
        TsunamiHighlightImage.SetActive(false);
        HealthBar.SetActive(true);
        deathText.SetActive(false);
        respawn.SetActive(false);
        returntomenu.SetActive(false);
        buttons.SetActive(false);
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

    public void DisableUI()
    {
        WildfireUI.SetActive(false);
        BlizzardUI.SetActive(false);
        TsunamiUI.SetActive(false);
        HealthBar.SetActive(false);
        WildfireHighlightImage.SetActive(false);
        BlizzardHighlightImage.SetActive(false);
        TsunamiHighlightImage.SetActive(false);
    }

    public void EnableDeathUI()
    {
        deathText.SetActive(true);
        respawn.SetActive(true);
        returntomenu.SetActive(true);
        buttons.SetActive(true);
    }

    public void DisableDeathUI()
    {
        deathText.SetActive(false);
        respawn.SetActive(false);
        returntomenu.SetActive(false);
        buttons.SetActive(false);
    }

    public void EnableUI(int equipped)
    {
        HealthBar.SetActive(true);
        WildfireUI.SetActive(true);
        BlizzardUI.SetActive(true);
        TsunamiUI.SetActive(true);
        switch (equipped)
        {
            case 0:
                SetBlizzardUI();
                break;
            case 1:
                SetTsunamiUI();
                break;
            case 2:
                SetWildfireUI();
                break;
            default:
                break;
        }
    }
}
