using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismBehaviour : ProjectileWeaponBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

     void MoveProjectile()
    {
        Vector3 newPosition = transform.position + direction * currentSpeed * Time.deltaTime;

        // get the screen boundaries
        Vector2 minScreenBounds = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector2 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        // check if the new position is outside the screen bounds
        if (newPosition.x < minScreenBounds.x || newPosition.x > maxScreenBounds.x)
        {
            // reflect the direction along the x-axis
            direction.x *= -1;
        }

        if (newPosition.y < minScreenBounds.y || newPosition.y > maxScreenBounds.y)
        {
            // reflect the direction along the y-axis
            direction.y *= -1;
        }

        // update the position
        transform.position = newPosition;
    }
}

