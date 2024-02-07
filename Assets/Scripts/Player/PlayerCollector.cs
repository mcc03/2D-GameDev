using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) 
    {
        //if object has the ICollectible interface, call collect function on it
        if (col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            collectible.Collect();
        }
    }
}
