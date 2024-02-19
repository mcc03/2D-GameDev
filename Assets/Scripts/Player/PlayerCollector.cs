using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{

    //access player stats variables
    PlayerStats player;

    //used to detect items within a range
    CircleCollider2D playerCollector;

    //magnet strength
    public float pullSpeed;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        playerCollector.radius = player.CurrentMagnet;
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        //if object has the ICollectible interface, call collect function on it
        if (col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>(); //gets rigidbody of the item
            Vector2 forceDirection = (transform.position - col.transform.position).normalized; //vector2 that points from the items position to the player pos
            rb.AddForce(forceDirection * pullSpeed); //adding force to the item

            collectible.Collect();
        }
    }
}
