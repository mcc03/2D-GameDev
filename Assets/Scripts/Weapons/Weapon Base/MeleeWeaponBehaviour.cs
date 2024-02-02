using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base script of all melee behaviours (to be placed on a prefab of a weapon that is melee)
public class MeleeWeaponBehaviour : MonoBehaviour
{

    public WeaponScriptableObject weaponData;

    public float destroyAfterSeconds;

    //current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    // apply damage to enemies 
    protected virtual void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
        }
    }
}
