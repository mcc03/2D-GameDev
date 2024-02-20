using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //holds players weapons and passive items
    public List <WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6); //stores weapon UI images
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6); //stores passive item UI images

    [System.Serializable]
    public class WeaponUpgrade // upgrade data for a weapon
    {
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade // upgrade data for a passive item
    {
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI // upgrade data for a single item
    {
        public Text upgradeNameDisplay;
        public Text UpgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>(); // list of upgrade options for weapon
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); // list of upgrade options for passive item
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>(); // list of upgrade options available in the UI 

    PlayerStats player; // refernce to player

    void Start()
    {
        player = GetComponent<PlayerStats>(); // get player stats variables
    }

    //assign weapon to the given slot index
    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon; //assign slot index to weapon
        weaponLevels[slotIndex] = weapon.weaponData.Level; //assigning the level to the weapon
        weaponUISlots[slotIndex].enabled = true; //only shows images with slots in use
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon; //set image to weapon of that slot index

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade) // end level up state
        {
            GameManager.instance.EndLevelUp();
        }
    }

    //assign passive weapon to the given slot index
    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem; //assign slot index to passive item
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level; //assign the level to the passive item
        passiveItemUISlots[slotIndex].enabled = true; //only shows images with slots in use
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon; //set image to passive item of that slot index

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade) // end level up state
        {
            GameManager.instance.EndLevelUp();
        }
    }

    //leveling system for weapons
    public void LevelUpWeapon(int slotIndex)
    {
        //check if inventory contains weapon in the slot index
        if(weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];

            //checking if current weapon has a next level prefab
            if(!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("No next level for" + weapon.name);
                return;
            }

            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform); //set the weapon as a child of player
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>()); //add the new weapon
            Destroy(weapon.gameObject); //destroy the old weapon
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level; //update the weapon level

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade) // end level up state
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    //leveling system for passive items
    public void LevelUpPassiveItem(int slotIndex)
    {
        //check if inventory contains passive item in the slot index
        if(passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];

            //checking if current passive item has a next level prefab
            if(!passiveItem.passiveItemData.NextLevelPrefab)
            {
                Debug.LogError("No next level for" + passiveItem.name);
                return;
            }

            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform); //set the passive item as a child of player
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>()); //add the new passive item
            Destroy(passiveItem.gameObject); //destroy the old passive item
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level; //update the passive item level

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade) // end level up state
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    void ApplyUpgradeOptions()
    {
        // loop through the upgrade options available
        foreach (var upgradeOption in upgradeUIOptions)
        {
            // determine whether upgrade is for a weapon or passive item
            int upgradeType = Random.Range(1, 3); // choose between weapon and passive item

            if(upgradeType == 1)
            {
                WeaponUpgrade chosenWeaponUpgrade = weaponUpgradeOptions[Random.Range(0, weaponUpgradeOptions.Count)];

                if(chosenWeaponUpgrade != null)
                {
                    bool newWeapon = false;

                    // checks weapon slots to see if it is a new weapon or already exists
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        // if the slot being currently iterated on is not null and matches the data of the chosen weapon upgrade, weapon already exists
                        if(weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;

                            if(!newWeapon)
                            {
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i)); // apply button functionality
                                // set description and name to that of the next level
                                upgradeOption.UpgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        } 
                    }
                    if(newWeapon) // spawn in weapon
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon)); // apply button functionality
                        // set initial description and name
                        upgradeOption.UpgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                    }

                    // set icon to the chosen weapon upgrade
                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            // repeat process for passive item
            else if(upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = passiveItemUpgradeOptions[Random.Range(0, passiveItemUpgradeOptions.Count)];

                if(chosenPassiveItemUpgrade != null)
                {
                    bool newPassiveItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if(passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
                        {
                            newPassiveItem = false;

                            if(!newPassiveItem)
                            {
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i)); 
                                // set description and name to that of the next level
                                upgradeOption.UpgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                            }
                            break;
                        }
                        else 
                        {
                            newPassiveItem = true;
                        }
                    }
                    if(newPassiveItem == true)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                        // set initial description and name
                        upgradeOption.UpgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
                    }
                    // set icon to the chosen weapon upgrade
                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }
}
