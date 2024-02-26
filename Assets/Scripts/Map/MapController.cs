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

    // used to track what layer is and is not the terrain
    public LayerMask terrainMask;

    public GameObject currentChunk;
    Vector3 playerLastPosition;

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
        //pm = FindObjectOfType<PlayerMovement>();
        playerLastPosition = player.transform.position;
        
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

        // instead of relying on move direction to spawn chunks, look at their current position compared to the last recorded position
        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position; // update position

        // get direction name
        string directionName = GetDirectionName(moveDir);

        CheckAndSpawnChunk(directionName);

        // spawn adjacent chunks when moving diagonally
        if(directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
        }
        if(directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
        }
        if(directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
        }
        if(directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
        }
    }

    void CheckAndSpawnChunk(string direction)
    {
        if(!Physics2D.OverlapCircle(currentChunk.transform.Find(direction).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(direction).position);
        }
    }

    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;

        // checking if the direction is primarily in the x or y
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // moving horizontally more than vertically
            if(direction.y > 0.5f)
            {
                return direction.x > 0 ? "Right Up" : "Left Up"; // moving moving up
            }
            else if(direction.y < -0.5f)
            {
                return direction.x > 0 ? "Right Down" : "Left Down"; // moving moving down
            }
            else
            {
                return direction.x > 0 ? "Right" : "Left"; // moving horizontally
            }
        }
        else
        {
            // moving vertically more than horizontally
            if(direction.x > 0.5f)
            {
                return direction.y > 0 ? "Right Up" : "Right Down"; // moving right
            }
            else if(direction.x < -0.5f)
            {
                return direction.y > 0 ? "Left Up" : "Left Down"; // moving left
            }
            else
            {
                return direction.y > 0 ? "Up" : "Down"; // moving vertically
            }
        }
    }

    // spawning chunks where there is not a chunk already
    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
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


        // if(pm.moveDir.x > 0 && pm.moveDir.y == 0) // moving right
        //     {
        //         // checking distance to spawn a chunk, tilemap is 20
        //         if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
        //             {
        //                 noTerrainPosition = currentChunk.transform.Find("Right").position;
        //                 SpawnChunk();
        //             }
        //     }   
        //     else if(pm.moveDir.x < 0 && pm.moveDir.y == 0) // moving left
        //         {
        //             if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
        //                 {
        //                     noTerrainPosition = currentChunk.transform.Find("Left").position;
        //                     SpawnChunk();
        //                 }
        //         }
        //     else if(pm.moveDir.x == 0 && pm.moveDir.y > 0) // moving up
        //         {
        //             if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
        //                 {
        //                     noTerrainPosition = currentChunk.transform.Find("Up").position;
        //                     SpawnChunk();
        //                 }
        //         } 
        //     else if(pm.moveDir.x == 0 && pm.moveDir.y < 0) // moving down
        //         {
        //             if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
        //                 {
        //                     noTerrainPosition = currentChunk.transform.Find("Down").position;
        //                     SpawnChunk();
        //                 }
        //         }
        //     else if(pm.moveDir.x > 0 && pm.moveDir.y > 0) // moving right up
        //         {
        //             if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask))
        //                 {
        //                     noTerrainPosition = currentChunk.transform.Find("Right Up").position;
        //                     SpawnChunk();
        //                 }
        //         }
        //     else if(pm.moveDir.x > 0 && pm.moveDir.y < 0) // moving right down
        //         {
        //             if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask))
        //                 {
        //                     noTerrainPosition = currentChunk.transform.Find("Right Down").position;
        //                     SpawnChunk();
        //                 }
        //         }
        //     else if(pm.moveDir.x < 0 && pm.moveDir.y > 0) // moving left up
        //         {
        //             if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask))
        //                 {
        //                     noTerrainPosition = currentChunk.transform.Find("Left Up").position;
        //                     SpawnChunk();
        //                 }
        //         }
        //     else if(pm.moveDir.x < 0 && pm.moveDir.y < 0) // moving left down
        //         {
        //             if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask))
        //                 {
        //                     noTerrainPosition = currentChunk.transform.Find("Left Down").position;
        //                     SpawnChunk();
        //                 }
        //         }  