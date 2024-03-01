using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
     [System.Serializable]
    public class Wave
    {   
        public string waveName;
        public List<EnemyGroup> enemyGroups;// A list of groups of enemies to spawn in this wave
        public int waveQuota; // The total number of enemies in this wave.
        public float spawnInterval; // The interval at which to Spawn enemies.
        public int spawnCount; // The number of enemies already spawned  in this wave.
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; // The number of enemies to spawn in this wave.
        public int spawnCount; // The number of enemies already spawned  in this wave.
        public GameObject enemyPrehab;

    }

     public List<Wave> waves; // A list of all the town 
     public int currentWaveCount; // The index of the current wave [Remember the lsit starts from 0]

     [Header("Spawner Attributes")]
     float spawnTimer; // Timer use to determine when to spawn the next enemy
    public int enemiesAlive;
    public int maxEnemiesAllowed; // The maximum number of enemies allowed on the map at once
    public bool maxEnemiesReached = false; //A flag indicating if the maxium number of enemies has been reached.
    public float waveInterval; //  The Interval between each wave
    bool isWaveActive = false;

    [Header("Spawn Position")]
    public List<Transform> relativesSpawnPoints; // A list to store all the relative SpawnPoints of enemies


     Transform player;
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive) // checks if wave has ended and the next wave should start
        {
            StartCoroutine(BeginNextWave());
            Debug.Log("NextWaveShouldStart");
        }

        spawnTimer += Time.deltaTime;

        //Debug.Log(currentWaveCount);

        // check if its time to spawn the next enemy  
        if(spawnTimer >= waves[currentWaveCount].spawnInterval) 
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {   
        isWaveActive = true;
        // wave for waveInterval seconds before starting the next wave
        yield return new WaitForSeconds(waveInterval);

        // If there are more waves to start to start after the current wave, move on to the next
        if(currentWaveCount < waves.Count - 1 )
        {
            isWaveActive = false;
            currentWaveCount++;
            CalculateWaveQuota();
            Debug.Log("NextWaveStartIsFalse");
        }

        Debug.Log("NextWaveCalled");
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {
        // checks if the minimum number of enemies in the wave has been spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota &&!maxEnemiesReached)
        {
            // Spawn each type of enemies until quota is filled
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // check if the minimum number of enemies of this type been spawned
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    // Limit the number of enemies that can be spawned at once
                    if(enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                    // spawns the enemies at random positions close to the player
                    Instantiate(enemyGroup.enemyPrehab, player.position + relativesSpawnPoints[Random.Range(0, relativesSpawnPoints.Count)].position,Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }
        // Resetb the maxEnemiesReached flag if the number of enemies alive has drop below the maximum amount
        if(enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
    // Call this function when an enemy is killed
   public void OnEnemyKilled()
   {
    //Decrement the number of enemies alive
    enemiesAlive--;
   }
}
