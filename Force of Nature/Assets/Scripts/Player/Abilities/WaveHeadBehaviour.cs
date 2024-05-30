using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;

public class WaveHeadBehaviour : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float dirX;
    [SerializeField] private PlayerDataScrObj playerData;
    [SerializeField] private GameObject waveBodyPrefab;
    [SerializeField] private float travelTime;
    private bool moving;
    private float travelTimer;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private float xTime;
    private float xTimer;
    List<GameObject> waveSegments = new List<GameObject>();
    LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        waveSegments.Clear();
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        dirX = playerData.faceDir;
        xTime = coll.bounds.size.x / speed;
        travelTimer = 0f;
        xTimer = 100f;
        moving = true;
        ground = LayerMask.GetMask("Platform");

    }

    public void ActivateHitbox()
    {
        coll.enabled = true;
    }

    public void DeactivateHitbox()
    {
        coll.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<EntityTakeDamage>().TakeAbilityDamage(playerData.waveDmg, 2);
        }
    }


    private void DissapearWave()
    {
        Destroy(gameObject);
    }

}
