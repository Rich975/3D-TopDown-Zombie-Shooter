using Unity.Burst.CompilerServices;
using UnityEngine;

public class Gun : Weapon
{

    [SerializeField] private GameObject gunModelPrefab; // Assign this in the Inspector

    private void Awake()
    {
        weaponName = "Gun";
        damage = 10f;
        range = 20f;
        fireRate = 0.5f;

        weaponModel = Instantiate(gunModelPrefab, transform);
        weaponModel.SetActive(false);

       

    }


    public override void Fire()
    {
        // Implement the shooting logic for the gun
        Debug.Log("Shooting the gun!");
        GameObject weapon = GameObject.FindGameObjectWithTag("RaySpawnPos");


        RaycastHit hit;
        Vector3 forwardDirection = weapon.transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(weapon.transform.position, forwardDirection, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(weapon.transform.position, hit.point, Color.yellow);
            Debug.Log("Hit: " + hit.collider.name);

            // Apply damage if the object hit implements IDamageable
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
        else
        {
            Debug.DrawLine(weapon.transform.position, weapon.transform.position + forwardDirection * 1000, Color.white);
            Debug.Log("No Hit");
        }
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

    public override void AttachToHand()
    {
        base.AttachToHand();
    }
}

