using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inheret from base class
public class ProjectileSpeedPassiveItem : PassiveItem 
{
    protected override void ApplyModifier()
    {
        //multiply current movespeed by value of the scriptable object
        player.CurrentProjectileSpeed *= 1 + passiveItemData.Multiplier / 20f;
    }
}
