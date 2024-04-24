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
        }
    }
}