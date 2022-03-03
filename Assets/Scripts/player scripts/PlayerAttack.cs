using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private weaponManager weapon_manager;

    public float fireRate=15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_prefab, spear_prefab;

    [SerializeField]
    private Transform arrow_bow_startposition;
    private void Awake()
    {
        weapon_manager = GetComponent<weaponManager>();
        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;
            }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot()
    {   //assault rifle
        if(weapon_manager.GetCurrentSelectedWeapon().fireType==WeaponFireType.MULTIPLE)
        {
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();
                
                BulletFired();
            }
        }
        //other weapons
        else
        {
            if(Input.GetMouseButtonDown(0))
            {  //handle axe
                if(weapon_manager.GetCurrentSelectedWeapon().tag==Tags.AXE_TAG)
                {
                    weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();
                }
                //handle shoot
                if(weapon_manager.GetCurrentSelectedWeapon().bulletType==WeaponBulletType.BULLET)
                {
                    weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                }
                //arrow or spear
                else 
                { 
                    if(is_Aiming)
                    {
                        weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();

                        if(weapon_manager.GetCurrentSelectedWeapon().bulletType==WeaponBulletType.ARROW)
                        {
                            ThrowArrowSpear(true);
                           //throw arrow
                        }
                        else if(weapon_manager.GetCurrentSelectedWeapon().bulletType==WeaponBulletType.SPEAR)
                        {
                            //throw spear
                            ThrowArrowSpear(false);
                        }
                    }
                }
            }
        }   
    }
    void ZoomInAndOut()
    {
        if (weapon_manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAimType.AIM)
        {   //hold right mouse button
            if(Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);
                crosshair.SetActive(false);
            }
            //release right mouse button
            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOON_OUT_ANIM);
                crosshair.SetActive(true);
            }
        }//if we need to zoom the weapon
        if(weapon_manager.GetCurrentSelectedWeapon().weapon_Aim==WeaponAimType.SELF_AIM)
        {
            if(Input.GetMouseButtonDown(1))
            {
                weapon_manager.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;

            }
            if (Input.GetMouseButtonUp(1))
            {
                weapon_manager.GetCurrentSelectedWeapon().Aim(false);
                is_Aiming = false;

            }
        }
    }//zoomin and out

    void ThrowArrowSpear(bool throwArrow)
    {
        if(throwArrow)
        {
            
            GameObject arrow=Instantiate(arrow_prefab);
            arrow.transform.position = arrow_bow_startposition.position;
            arrow.GetComponent<AroowBowScript>().Launch(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spear_prefab);
            spear.transform.position = arrow_bow_startposition.position;
            spear.GetComponent<AroowBowScript>().Launch(mainCam);
        }
    }

    void BulletFired()
    {
        RaycastHit hit;

        if(Physics.Raycast(mainCam.transform.position,mainCam.transform.forward,out hit))
        {
            if(hit.transform.tag==Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<healthScript>().ApplyDamage(damage);
            }
        }
    }
}
