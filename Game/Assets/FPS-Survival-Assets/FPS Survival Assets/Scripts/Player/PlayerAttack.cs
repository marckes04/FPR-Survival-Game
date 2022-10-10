using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager weapon_manager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage;

    private Animator ZoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;

    [SerializeField]
    private Transform arrow_Bow_StartPosition;


    void Awake()
    {
        weapon_manager = GetComponent<WeaponManager>();

        ZoomCameraAnim = transform.Find(Tags.LOOK_ROOT).
                            transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Weaponshoot();
        ZoomInAndOut();
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

                BulletFired();
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

                    BulletFired();
                }
                else
                {
                    // We have an arrow or spear

                    if (is_Aiming)
                    {
                        weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();

                        if (weapon_manager.GetCurrentSelectedWeapon().bullet_type
                            == WeaponBulletType.ARROW)
                        {
                            // Throw Arrow

                            ThrowArrowOrSpear(true);

                        } else if (weapon_manager.GetCurrentSelectedWeapon().bullet_type
                            == WeaponBulletType.SPEAR)
                          {
                            //throw Spear
                            ThrowArrowOrSpear(false);

                          }
                    }

                }
            }


        }
        
    }

    void ZoomInAndOut()
    {
        // we are going to aim our camera on the weapon
        if (weapon_manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            // if we press and hold right button
            if (Input.GetMouseButtonDown(1))
            {

                ZoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);

                crosshair.SetActive(false);
            }


            // When we release right mouse click button
            if (Input.GetMouseButtonUp(1))
            {

                ZoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);

                crosshair.SetActive(true);
            }
        }

    if(weapon_manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weapon_manager.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                weapon_manager.GetCurrentSelectedWeapon().Aim(false);
                is_Aiming = false;
            }


        }// Weapon self AIM
    }// zoom in and out

    void ThrowArrowOrSpear(bool ThrowArrow)
    {
        if (ThrowArrow)
        {
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = arrow_Bow_StartPosition.position;

            arrow.GetComponent<ArrowAndBow>().Launch(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrow_Bow_StartPosition.position;

            spear.GetComponent<ArrowAndBow>().Launch(mainCam);

        }
    }// Throw Arrow or Speed

    void BulletFired()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCam.transform.position,mainCam.transform.forward, out hit))
        {
            
        }
    }

}

