using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject prefab;
    
    // getters and setters
    public GameObject Prefab { get => prefab; private set => prefab = value; }
    
    //base stats for weapons
    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value;}
    
    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }
    
    [SerializeField]
    float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }
    
    [SerializeField]
    int pierce;
    public int Pierce { get => pierce; private set => pierce = value; }

    //for editor
    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }

    //leveling of weapons (prefab for the next level of the weapon)
    [SerializeField]
    GameObject nextLevelPrefab;
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    //for displaying sprite (cannot be changed in game)
    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }
}
