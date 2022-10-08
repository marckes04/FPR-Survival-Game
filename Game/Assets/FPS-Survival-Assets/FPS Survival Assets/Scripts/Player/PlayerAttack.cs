using System.Collections;
using System.Collections.Generic;
using UnityEngine;
     

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager weapon_manager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage;

    void Awake()
    {
        weapon_manager = GetComponent<WeaponManager>();
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Weaponshoot();
    }

    void Weaponshoot()
    {
        //If we have Assault riffle
        if (weapon_manager.GetCurrentSelectedWeapon().fire_type == WeaponFireType.MULTIPLE)
        {
            //If we press and hold left mouse click AND
            // if time is greater
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();

                //Bullet Fired
            }
        }
        // If we have a regular weapon that shoots once 
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Handle Axe

                if (weapon_manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();
                }
              
                // Handle Shoot 

                if(weapon_manager.GetCurrentSelectedWeapon().bullet_type == WeaponBulletType.BULLET)
                {
                    weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();

                    //BulletFired();
                }
                else
                {
                    // We have an arrow or spear

                }
            }


        }
        

    }
}

