using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    // Start is called before the first frame update

    private SpriteRenderer deathBGSprite;
    public UIManager ui;
    public PlayerDataScrObj playerData;
    void Start()
    {
        deathBGSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void OnDeath()
    {
        deathBGSprite.enabled = true;
        ui.DisableUI();
        ui.EnableDeathUI();
    }

    public void OnRespawn()
    {
        deathBGSprite.enabled = false;
        ui.EnableUI(playerData.equipped);
        ui.DisableDeathUI();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
