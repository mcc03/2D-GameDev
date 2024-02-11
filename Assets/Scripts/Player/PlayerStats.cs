using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    //reference to char scriptable object
    public CharacterScriptableObject characterData;

    //current stats
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnet;

    //experience and level of the player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //allows class to be seen by unity
    //class for defining level range and experience cap increase
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public List<LevelRange> levelRanges;

    //I-Frames so the player does not instantly die when coming in contact with an enemy
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    void Awake()
    {
        //assign the variables
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;
    }

    public void Start()
    {
        //experience cap starts at 0
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update()
    {
        //if invincibility timer is greather than 0, start to reduce it
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible) // if invinciblityTimer is not above 0, player is not immune to damage
        {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        //if experience is greater than current exp cap and current exp will be reduced by current exp cap, then increase next level exp cap
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }

    //method to allow the player to take damage from enemies
    public void TakeDamage(float dmg)
    {
        //if the player is not currently invincible, can take damage then start invincibiltyDuration
        if(!isInvincible)
        {
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if(currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("You are dead!");
    }

    //healing
    public void RestoreHealth(float amount)
    {
        //heal player if health is below max health
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;

            //make sure the player's health does not exceed max health
            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    // recover health
    void Recover()
    {
        if(currentHealth < characterData.MaxHealth)
        {
            //increase current health
            currentHealth += currentRecovery * Time.deltaTime;

            //ensuring that health recovered does not exceed maximum health cap
            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }
}
