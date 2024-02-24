using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    //reference to char scriptable object
    CharacterScriptableObject characterData;

    //current stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;

    float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            // check if value has changed
            if(currentHealth != value)
            {
                currentHealth = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth; // show stat UI
                }
                //update the value in real time
            }
        }
    }

    #region Current Stats Properties
    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            // check if value has changed
            if(currentRecovery != value)
            {
                currentRecovery = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery; // show stat UI
                }
                //update the value in real time
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            // check if value has changed
            if(currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed; // show stat UI
                }
                //update the value in real time
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            // check if value has changed
            if(currentMight != value)
            {
                currentMight = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight; // show stat UI
                }
                //update the value in real time
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            // check if value has changed
            if(currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed; // show stat UI
                }
                //update the value in real time
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            // check if value has changed
            if(currentMagnet != value)
            {
                currentMagnet = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet; // show stat UI
                }
                //update the value in real time
            }
        }
    }
    #endregion

    //weapons you can spawn with
    //public List<GameObject> spawnedWeapons;

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

    //I-Frames so the player does not instantly die when coming in contact with an enemy
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoryManager inventory; //reference to inv managaer
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public Text levelText;

    //for testing
    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;

    void Awake()
    {

        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        //assign the variables
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;

        //spawn the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
        //SpawnWeapon(secondWeaponTest);
        //SpawnPassiveItem(firstPassiveItemTest);
        SpawnPassiveItem(secondPassiveItemTest);
    }

    public void Start()
    {
        //experience cap starts at 0
        experienceCap = levelRanges[0].experienceCapIncrease;

        // set current stats to display
        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar(); // set health bar value
        UpdateExpBar(); // set exp value
        UpdateLevelText(); // set lvl value
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

        UpdateExpBar();
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

            UpdateLevelText();

            GameManager.instance.StartLevelUp(); // trigger start level up method
        }
    }

    void UpdateExpBar()
    {
        // update exp bar fill amount
        expBar.fillAmount = (float)experience / experienceCap; // divide players experience by the current exp cap
    }

    void UpdateLevelText()
    {
        // update level text
        levelText.text = " LV " + level.ToString();
    }

    //method to allow the player to take damage from enemies
    public void TakeDamage(float dmg)
    {
        //if the player is not currently invincible, can take damage then start invincibiltyDuration
        if(!isInvincible)
        {
            CurrentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if(CurrentHealth <= 0)
            {
                Kill();
            }

            UpdateHealthBar(); // update health bar after taking damage
        }
    }

    void UpdateHealthBar()
    {
        // update health bar
        healthBar.fillAmount = currentHealth / characterData.MaxHealth; // calculating players ratio to current health to max health
    }

    public void Kill()
    {
        if(!GameManager.instance.isGamerOver)
        {
            GameManager.instance.AssignLevelReachedUI(level); // assign players' level reached
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots); // assign corresponding sprites
            GameManager.instance.GameOver(); // called when game is over
        }
    }

    //healing
    public void RestoreHealth(float amount)
    {
        //heal player if health is below max health
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;

            //make sure the player's health does not exceed max health
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    // recover health
    void Recover()
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            //increase current health
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            //ensuring that health recovered does not exceed maximum health cap
            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon) 
    {
        //checking if inventory slots are full
        if(weaponIndex >= inventory.weaponSlots.Count -1) //-1 because lists starts at 0
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        //spawn the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); //weapon set to be child of the player
        //spawnedWeapons.Add(spawnedWeapon); //add it to the list of spawned weapons
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>()); //add weapon to the inventory slot

        weaponIndex++; //prevents overlapping -weapons need to be assinged to seperate slots
    }

        public void SpawnPassiveItem(GameObject passiveItem) 
    {
        //checking if inventory slots are full
        if(passiveItemIndex >= inventory.passiveItemSlots.Count -1) //-1 because lists starts at 0
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        //spawn the starting passive item
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); //weapon set to be child of the player
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); //add weapon to the inventory slot

        passiveItemIndex++; //prevents overlapping -weapons need to be assinged to seperate slots
    }    
}
