using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// setting parent
public class GarlicController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spanwedGarlic = Instantiate(prefab);
        spanwedGarlic.transform.position = transform.position; // garlic will follow the player
        spanwedGarlic.transform.parent = transform; // so it is layered underneath the player
    }

}
