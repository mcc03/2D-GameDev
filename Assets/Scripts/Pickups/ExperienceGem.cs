using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup
{

    //how much experience the gem gives
    public int experienceGranted;


    public override void Collect()
    {
        if(hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }

        //Debug.Log("Collected");
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
