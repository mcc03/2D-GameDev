using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inheret from base class
public class RecoveryPassiveItem : PassiveItem 
{
    protected override void ApplyModifier()
    {
        //multiply current movespeed by value of the scriptable object
        player.CurrentRecovery *= 1 + passiveItemData.Multiplier / 10f;
    }
}

