using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //used to reference prop locations
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        SpawnProps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach (GameObject sp in propSpawnPoints)
            {
                // random number between 0 and number of prop prefabs
                int rand = Random.Range(0, propPrefabs.Count);
                // spawning the props and making sure they have no rotation and parents the prop inside the spawn location
                GameObject prop = Instantiate(propPrefabs[rand], sp.transform.position, Quaternion.identity);
                prop.transform.parent = sp.transform;
            }
    }
}
