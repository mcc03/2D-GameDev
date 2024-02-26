using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, ICollectible
{
    protected bool hasBeenCollected = false; // this is required so a the same pickup cannot be collected multiple times

    public virtual void Collect()
    {
        hasBeenCollected = true;
    }

    //destroy object when it collides with player
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
