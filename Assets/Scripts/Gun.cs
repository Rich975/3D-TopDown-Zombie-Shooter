using UnityEngine;

public class Gun : Weapon
{
    private void Awake()
    {
        weaponName = "Gun";
        damage = 10f;
        range = 20f;
        attackRate = 0.5f;
    }

    public override void Use()
    {
        // Implement the shooting logic for the gun
        Debug.Log("Shooting the gun!");
        // Additional code to shoot bullets and deal damage to enemies
    }

    public override void Equip()
    {
        base.Equip();
        // Additional code specific to equipping the gun
    }

    public override void Unequip()
    {
        base.Unequip();
        // Additional code specific to unequipping the gun
    }
}

