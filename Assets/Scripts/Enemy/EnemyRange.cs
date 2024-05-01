/* using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

[CreateAssetMenu(fileName="EnemyScriptableObject", menuName="ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    // Base stats for enemies
    [SerializeField] 
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }

    [SerializeField]
    float maxHealth; 
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    [SerializeField] 
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField] 
    float attackRange;
    public float AttackRange { get => attackRange; private set => attackRange = value; }

    [SerializeField]
    bool isRangedAttacker;
    public bool IsRangedAttacker { get => isRangedAttacker; private set => isRangedAttacker = value; }
} */