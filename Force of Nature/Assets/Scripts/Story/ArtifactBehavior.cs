using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactBehavior : MonoBehaviour
{
    [SerializeField] private PlayerDataScrObj playerData;

    void Start()
    {
        playerData.abilitiesUnlocked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerData.abilitiesUnlocked = true;
            collision.gameObject.GetComponent<PlayerAbilities>().SetColor();
            Destroy(gameObject);
        }
    }
}
