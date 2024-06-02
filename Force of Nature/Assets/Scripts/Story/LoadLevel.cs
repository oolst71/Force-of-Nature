using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> entityList = new List<GameObject>();
    public List<Vector3> entityLocations = new List<Vector3>();

    void Start()
    {
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("Respawnable");
        for (int i = 0; i < respawns.Length; i++)
        {
            entityList.Add(respawns[i]);
            entityLocations.Add(respawns[i].transform.position);
        }
        //get all entities in the scene, save their locations
    }


    public void ReloadLevel()
    {
        for (int i = 0; i < entityList.Count; i++)
        {
            entityList[i].transform.position = entityLocations[i];
            EntityTakeDamage etd = entityList[i].gameObject.GetComponent<EntityTakeDamage>();
            etd.health = etd.enemyData.maxHealth;
            entityList[i].gameObject.SetActive(true);
        }
        //loop through the list of entities
        //reset them to their saved locations and maximum hp, set them to active
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
