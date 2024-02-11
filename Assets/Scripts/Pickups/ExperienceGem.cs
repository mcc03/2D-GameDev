using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup, ICollectible
{

    //how much experience the gem gives
    public int experienceGranted;


    public void Collect()
    {
        //Debug.Log("Collected");
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
