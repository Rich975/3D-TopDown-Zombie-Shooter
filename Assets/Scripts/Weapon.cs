using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public float range;
    public float attackRate;

    [SerializeField] private GameObject weaponModel; // Reference to the weapon's 3D model

    // Abstract method to use the weapon, to be implemented by specific weapon types
    public abstract void Use();

    // Method to equip the weapon and show its model
    public virtual void Equip()
    {
        if (weaponModel != null)
        {
            weaponModel.SetActive(true);
        }
        Debug.Log(weaponName + " equipped.");
    }

    // Method to unequip the weapon and hide its model
    public virtual void Unequip()
    {
        if (weaponModel == null)
        {
            weaponModel.SetActive(false);
        }
        Debug.Log(weaponName + " unequipped.");
    }
}


