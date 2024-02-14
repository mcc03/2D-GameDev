using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //holds players weapons and passive items
    public List <WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevel = new int[6];

    //assign weapon to the given slot index
    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
    }

    //assign passive weapon to the given slot index
    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
    }

    //leveling system for weapons
    public void LevelUpWeapon(int slotIndex)
    {

    }

    //leveling system for passive items
    public void LevelUpPassiveItem(int slotIndex)
    {

    }
}
