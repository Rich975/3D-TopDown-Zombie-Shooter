using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab; // The weapon to be added to the player's inventory

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                PlayerInventory inventory = playerController.GetComponent<PlayerInventory>();
                if (inventory != null && weaponPrefab != null)
                {
                    // Instantiate the weapon and add it to the inventory
                    Weapon weaponInstance = Instantiate(weaponPrefab);
                    inventory.AddWeapon(weaponInstance);
                    // Optionally, destroy or deactivate the pickup object
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
