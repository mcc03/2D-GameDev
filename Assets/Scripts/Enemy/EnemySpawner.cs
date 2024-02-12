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

     public float waveInterval; //  The Interval between each wave


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
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 ) // checks if wave has ended and the next wave should start
        {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer += Time.deltaTime;

        // check if its time to spawn the next enemy  
        if(spawnTimer >= waves[currentWaveCount].spawnInterval) 
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

        IEnumerator BeginNextWave()
    {   
        // wave for waveInterval seconds before starting the next wave
        yield return new WaitForSeconds(waveInterval);

        // If there are more waves to start to start after the current wave, move on to the next
        if(currentWaveCount < waves.Count - 1  )
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
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
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
        {
            // Spawn each type of enemies until quota is filled
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // check if the minimum number of enemies of this type been spawned
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    Instantiate(enemyGroup.enemyPrehab, spawnPosition, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                }
            }
        }
    }
}
