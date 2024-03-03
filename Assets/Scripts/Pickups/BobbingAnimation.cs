using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency; // animation speed
    public float magnitude; // range of motion
    public Vector3 direction; // direction of animation
    Vector3 initialPosition;

    Pickup pickup; // reference pickup script

    // offset animation so they are not all synced
    float animationOffsetTime;

    private void Start()
    {
        pickup = GetComponent<Pickup>();

        // save the initial position value
        initialPosition = transform.position;

        // random offset time for the object
        animationOffsetTime = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        if(pickup && !pickup.hasBeenCollected) // check if hasCollected is false
        {
        // use sine for smooth animation (mathf.sin creates a wave animation)
        transform.position = initialPosition + direction * Mathf.Sin((Time.time + animationOffsetTime) * frequency) * magnitude;
        }
    }
}
