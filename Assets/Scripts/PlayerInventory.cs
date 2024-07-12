using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public List<Weapon> weapons = new List<Weapon>(); // List to store weapons
    private int currentWeaponIndex = -1;// Index of the currently equipped weapon


    // Add a weapon to the inventory
    public void AddWeapon(Weapon weapon) 
    {
        weapons.Add(weapon);
        if(currentWeaponIndex == -1)
        {
            currentWeaponIndex = 0;
            ToggleWeapon(currentWeaponIndex);
        }
    }

    // Toggle a weapon by index
    public void ToggleWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            if (currentWeaponIndex == index)
            {
                // If the selected weapon is already equipped, unequip it
                weapons[currentWeaponIndex].Unequip();
                currentWeaponIndex = -1; // No weapon is currently equipped
            }
            else
            {
                // If a different weapon is equipped, unequip it
                if (currentWeaponIndex != -1)
                {
                    weapons[currentWeaponIndex].Unequip();
                }
                // Equip the selected weapon
                currentWeaponIndex = index;
                weapons[currentWeaponIndex].Equip();
            }
        }
    }

    // Use the currently equipped weapon
    public void UseCurrentWeapon()
    {
        if (currentWeaponIndex != -1)
        {
            weapons[currentWeaponIndex].Use();
        }
    }

    // Get the currently equipped weapon
    public Weapon GetCurrentWeapon()
    {
        if (currentWeaponIndex != -1)
        {
            return weapons[currentWeaponIndex];
        }
        return null;
    }
}

