using UnityEngine;
using UnityEngine.Rendering;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public float range;
    public float fireRate;

    protected GameObject weaponModel; // Reference to the weapon's 3D model

    // Abstract method to use the weapon, to be implemented by specific weapon types
    public abstract void Fire();

    // Method to equip the weapon and show its model
    public virtual void Equip()
    {
        if (weaponModel != null)
        {

            weaponModel.SetActive(true);
            AttachToHand();
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

    // Method to attach the weapon to the player's hand (to be overridden if needed)
    public virtual void AttachToHand()
    {
        // Assuming the player's hand has a known transform
        Transform handTransform = GameObject.FindWithTag("PlayerHand").transform;
        weaponModel.transform.SetParent(handTransform);
        weaponModel.transform.localPosition = Vector3.zero;
        Vector3 customRot = new Vector3(0f, 0f, 0f);
        weaponModel.transform.localRotation = Quaternion.identity;
    }
}


