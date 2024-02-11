using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //destroy object when it collides with player
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
