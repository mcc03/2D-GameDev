using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base script of all projectile behviours (to be placed on a prefab of a weapon that is a projectile)
public class ProjectileWeaponBehaviour : MonoBehaviour
{

    protected Vector3 direction;

    // remove the object after set time
    public float destroyAfterSeconds;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        // rotating of knife based on x and y movement
        if(dirx < 0 && diry == 0) // left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirx == 0 && diry < 0) // down
        {
            scale.y = scale.y * -1;
        }
        else if (dirx == 0 && diry > 0) // up
        {
            scale.x = scale.x * -1;
        }
        else if (dir.x > 0 && dir.y > 0) // right up
        {
            rotation.z = 0f;
        }
        else if (dir.x > 0 && dir.y < 0) // right down
        {
            rotation.z = -90f;
        }
        else if (dir.x < 0 && dir.y > 0) // left up
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dir.x < 0 && dir.y < 0) // left down
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 0F;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation); // cannot set vector because cannot convert a quaternion to a Vector3
    }
}
