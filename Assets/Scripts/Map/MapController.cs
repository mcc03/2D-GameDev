using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    // used to store the terrain chunks
    public List<GameObject> terrainChunks;

    // used to reference location of player
    public GameObject player;

    public float checkerRadius;

    // to determine where this is NOT a chunk
    Vector3 noTerrainPosition;

    // used to track what layer is and is not the terrain
    public LayerMask terrainMask;

    public GameObject currentChunk;

    // used to access variables
    PlayerMovement pm;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist; // must be greater than the length and width of the tilemap
    float opDist; // reference current distance for each of the chunks from the player
    float optimizerCooldown; // reduce amount of times the optimizer calculation is running
    public float optimizerCooldownDur;

    

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    // this performs the checks whether a chunk should be spawned or not in all directions
    void ChunkChecker()
    {   
        if(!currentChunk)
        {
            return;
        }

        if(pm.moveDir.x > 0 && pm.moveDir.y == 0) // moving right
            {
                // checking distance to spawn a chunk, tilemap is 20
                if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
                    {
                        noTerrainPosition = currentChunk.transform.Find("Right").position;
                        SpawnChunk();
                    }
            }   
            else if(pm.moveDir.x < 0 && pm.moveDir.y == 0) // moving left
                {
                    if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
                        {
                            noTerrainPosition = currentChunk.transform.Find("Left").position;
                            SpawnChunk();
                        }
                }
            else if(pm.moveDir.x == 0 && pm.moveDir.y > 0) // moving up
                {
                    if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
                        {
                            noTerrainPosition = currentChunk.transform.Find("Up").position;
                            SpawnChunk();
                        }
                } 
            else if(pm.moveDir.x == 0 && pm.moveDir.y < 0) // moving down
                {
                    if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
                        {
                            noTerrainPosition = currentChunk.transform.Find("Down").position;
                            SpawnChunk();
                        }
                }
            else if(pm.moveDir.x > 0 && pm.moveDir.y > 0) // moving right up
                {
                    if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask))
                        {
                            noTerrainPosition = currentChunk.transform.Find("Right Up").position;
                            SpawnChunk();
                        }
                }
            else if(pm.moveDir.x > 0 && pm.moveDir.y < 0) // moving right down
                {
                    if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask))
                        {
                            noTerrainPosition = currentChunk.transform.Find("Right Down").position;
                            SpawnChunk();
                        }
                }
            else if(pm.moveDir.x < 0 && pm.moveDir.y > 0) // moving left up
                {
                    if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask))
                        {
                            noTerrainPosition = currentChunk.transform.Find("Left Up").position;
                            SpawnChunk();
                        }
                }
            else if(pm.moveDir.x < 0 && pm.moveDir.y < 0) // moving left down
                {
                    if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask))
                        {
                            noTerrainPosition = currentChunk.transform.Find("Left Down").position;
                            SpawnChunk();
                        }
                }                   
    }

    // spawning chunks where there is not a chunk already
    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDur;
        }
        else 
        {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            // setting opDist the distance between player and chunk
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
