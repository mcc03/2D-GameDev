using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : MonoBehaviour, ICollectible
{

    //how much experience the gem gives
    public int experienceGranted;


    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
        
        //destroy gem once picked up
        Destroy(gameObject);
    }
}
