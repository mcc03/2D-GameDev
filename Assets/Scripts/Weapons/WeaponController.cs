using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base script for all weapon controllers
public class WeaponController : MonoBehaviour
{
    
    [Header("Weapon Stats")]
    public GameObject prefab;
    public float damage;
    public float speed;
    public float cooldownDuration;
    float currentCooldown;
    public int pierce;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = cooldownDuration; // at start set current cooldown to the duration
    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f) // once cooldown reaches 0, attack
        {
            Attack();
        }
    }

    void Attack()
    {
        currentCooldown = cooldownDuration;
    }
}
