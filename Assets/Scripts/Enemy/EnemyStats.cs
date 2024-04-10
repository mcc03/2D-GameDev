using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SpriteRenderer))]

public class EnemyStats : MonoBehaviour
{
    // reference
    private KillCounter kc;
    //reference for enemy scriptable object
    public EnemyScriptableObject enemyData;
    //variables for enemy scriptable object
    //current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    public static int playerScoreKills = 0;
    public float despawnDistance = 20f;
    Transform player;
    


    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    private void Start()
    {
        kc = GameObject.Find("KCO").GetComponent<KillCounter>();
    }

    //allows us to damage the enemy
    public void TakeDamage(float dmg)
    {
        // create text popup when enemy takes damage
        if (dmg > 0)
        {
            GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
        }

        currentHealth -= dmg;

        if(currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);

        playerScoreKills++;
        Debug.Log("Current Score: " + playerScoreKills);

        kc.AddKill(); // update count
        kc.ShowKills(); // display count
    }

    //enemy will deal damager to player when their collider is touching the players' collider
    private void OnCollisionStay2D(Collision2D col) 
    {
        //reference script from the collided collider and deal damage using TakeDamage method
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }
}
