using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// knife controller has access to weapon controller stuff
public class PrismController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedPrism = Instantiate(weaponData.Prefab);
        spawnedPrism.transform.position = transform.position; // assign position to be the same as this object with is parented to the player
        spawnedPrism.GetComponent<PrismBehaviour>().DirectionChecker(pm.lastMovedVector); // reference and set the direction
    }
}
