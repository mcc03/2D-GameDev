using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //holds players weapons and passive items
    public List <WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];

    //assign weapon to the given slot index
    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon; //assign slot index to weapon
        weaponLevels[slotIndex] = weapon.weaponData.Level; //assigning the level to the weapon
    }

    //assign passive weapon to the given slot index
    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem; //assign slot index to passive item
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level; //assign the level to the passive item
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
        }
    }
}
