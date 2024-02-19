using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inheret from base class
public class WingsPassiveItem : PassiveItem 
{
    protected override void ApplyModifier()
    {
        //multiply current movespeed by value of the scriptable object
        player.CurrentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }
}
