using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class EnemyRandomMovement : MonoBehaviour
{
  //reference the enemy stats
  EnemyStats enemy;
  Transform player;

  // Start is called before the first frame update
  void Start()
  {
    enemy = GetComponent<EnemyStats>();
    player = FindObjectOfType<PlayerMovement>().transform; 
  }

  // Update is called once per frame
  void Update()
  {
    // Generate a random direction
    Vector3 randomDirection = Random.insideUnitSphere * enemy.currentMoveSpeed;

    // 20% of the time move towards the player
    if(Random.Range(0, 100) < 20) {
      randomDirection = (player.position - transform.position).normalized * enemy.currentMoveSpeed; 
    }

    // Move in the random direction
    transform.Translate(randomDirection * Time.deltaTime);
  }
}