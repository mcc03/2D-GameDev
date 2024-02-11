using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Pickup, ICollectible
{

    //amount of health to restore to player
    public int healthToRestore;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth(healthToRestore);
    }
}
