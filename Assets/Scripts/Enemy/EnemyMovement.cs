using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    // Reference the enemy stats
    EnemyStats enemy;
    Transform player;
    
    // For flipping sprite
    SpriteRenderer spriteRenderer;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() 
    {
        // Calculate direction between enemy and player
        Vector2 direction = player.position - transform.position;
        
        // Flip sprite to face towards player
        if(direction.x < 0) {
            spriteRenderer.flipX = true; 
        }
        else {
            spriteRenderer.flipX = false;
        }
        
        // Move towards player
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);
    }

}