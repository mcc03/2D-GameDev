using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    //reference player stats so we can modify them
    protected PlayerStats player;
    //reference to passive item scriptable object to modify values
    public PassiveItemScriptableObject passiveItemData; 

    protected virtual void ApplyModifier()
    {
        //apply the value to the correct stat in child classes
    }
    
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }
}
