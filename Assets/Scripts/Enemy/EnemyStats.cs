using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
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

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
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
