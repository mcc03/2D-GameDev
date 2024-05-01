using System.Collections;
using System.Collections.Generic;  
using UnityEngine;

public class EnemyRangeMovement : MonoBehaviour
{

    //reference the enemy stats
    EnemyStats enemy;
    Transform player;

    public float stopDistance = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        
        if(distance > stopDistance) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);
            
            // Check if enemy is facing away from player
            float dotProduct = Vector2.Dot(transform.right, (player.position - transform.position).normalized);
            if(dotProduct < 0f) {
                // Enemy is facing away, so reverse sprite
                GetComponent<SpriteRenderer>().flipX = true; 
            }
            else {
                // Enemy is facing towards player, so don't flip
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
}