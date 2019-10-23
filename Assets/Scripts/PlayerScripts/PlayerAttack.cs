using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager weaponManager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;

    private bool is_Aiming;

    private Camera mainCam;

    // 十字准星
    private GameObject crosshair;

    // arrow and spear
    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;

    // arrow&spear spawn postion
    [SerializeField]
    private Transform arrow_Bow_StartPostion;



    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();

        zoomCameraAnim = transform.Find(Tag.LOOK_ROOT).transform.Find(Tag.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tag.CROSSHAIR);

        mainCam = Camera.main;
    }

    void Start()
    {
        
    }

    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot()
    {

        // hold and auto fire
        if(weaponManager.GetCurrentWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                weaponManager.GetCurrentWeapon().ShootAnimation();
                BulletFired();
            }
        }
        // one click one shot
        else
        {
            if (Input.GetMouseButtonDown(0))
            {

                // axe
                if(weaponManager.GetCurrentWeapon().tag == Tag.AXE_TAG)
                {
                    weaponManager.GetCurrentWeapon().ShootAnimation();
                }

                // bullet
                if (weaponManager.GetCurrentWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weaponManager.GetCurrentWeapon().ShootAnimation();
                    BulletFired();
                }
                // bow and spear
                else
                {
                    if (is_Aiming)
                    {
                        if(weaponManager.GetCurrentWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            // throw arrow
                            ThrowArrowOrSpear(true);
                        }
                        else if(weaponManager.GetCurrentWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            // throw spear
                            ThrowArrowOrSpear(false);
                        }
                    }
                    else
                    {

                    }
                }
            }

                
        }

    }


    void ZoomInAndOut()
    {
        if (weaponManager.GetCurrentWeapon().weaponAim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTag.ZOOM_IN_ANIM);
                //crosshair.SetActive(false);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(AnimationTag.ZOOM_OUT_ANIM);
                //crosshair.SetActive(true);
            }
        }

        if(weaponManager.GetCurrentWeapon().weaponAim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weaponManager.GetCurrentWeapon().Aim(true);
                is_Aiming = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                weaponManager.GetCurrentWeapon().Aim(false);
                is_Aiming = false;
            }
        }
    }


    void ThrowArrowOrSpear(bool throwArrow)
    {
        if (throwArrow)
        {
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = arrow_Bow_StartPostion.position;
            arrow.GetComponent<ArrowAndBowScript>().Launch(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrow_Bow_StartPostion.position;
            spear.GetComponent<ArrowAndBowScript>().Launch(mainCam);
        }
    }

    void BulletFired()
    {
        RaycastHit hit;
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * 30, Color.red, 2, false);
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            Debug.Log("hit： " + hit.transform.name + ", tag: " + hit.transform.tag);
            if(hit.transform.tag == Tag.ENEMY_TAG)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }

        }
    }
}
