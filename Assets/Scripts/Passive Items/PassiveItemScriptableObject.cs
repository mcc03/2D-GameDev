using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName ="ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{

    //passive item effects
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }

    //for editor
    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }

    //leveling of weapons (prefab for the next level of the weapon)
    [SerializeField]
    GameObject nextLevelPrefab;
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

}
