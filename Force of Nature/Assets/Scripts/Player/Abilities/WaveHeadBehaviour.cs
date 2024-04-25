using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    // Start is called before the first frame update
    void Start()
    {
        waveSegments.Clear();
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        dirX = playerData.faceDir;
        xTime = coll.bounds.size.x / speed;
        travelTimer = 0f;
        xTimer = 100f;
        moving = true;
    }

    private void Update()
    {
        travelTimer += Time.deltaTime;
        xTimer += Time.deltaTime;
        if (xTimer >= xTime)
        {
            GameObject segment = Instantiate(waveBodyPrefab, transform.position, Quaternion.identity);
            waveSegments.Add(segment);
            xTimer = 0f;
        }
        if (travelTimer >= travelTime)
        {
            moving = false;
            DissapearWave();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<EntityTakeDamage>().TakeAbilityDamage(playerData.waveDmg, 1);
        }
    }

    private void DissapearWave()
    {
        for (int i = 0; i < waveSegments.Count; i++)
        {
            Destroy(waveSegments[i]);
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            rb.velocity = new Vector2(speed * dirX, 0);
        }
    }
}
