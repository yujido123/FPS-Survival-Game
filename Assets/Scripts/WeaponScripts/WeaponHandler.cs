using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponAim
{
    NONE,
    SELF_AIM,
    AIM
}

public enum WeaponFireType
{
    SINGLE,
    MULTIPLE
}

public enum WeaponBulletType
{
    BULLET,
    ARROW,
    SPEAR,
    NONE
}


public class WeaponHandler : MonoBehaviour
{

    private Animator anim;

    [SerializeField]
    private GameObject muzzleFlash;

    public WeaponAim weaponAim;
    public WeaponFireType fireType;
    public WeaponBulletType bulletType;

    // sound
    [SerializeField]
    private AudioSource shoot_sound, reload_sound;

    public GameObject attack_Point;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    
    // Attack
    public void ShootAnimation()
    {
        anim.SetTrigger(AnimationTag.SHOOT_TRIGGER);
    }

    // Aim
    public void Aim(bool canAim)
    {
        anim.SetBool(AnimationTag.AIM_PARAMETER, canAim);
    }

    // muzzle flash
    public void Turn_On_MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
    }
    public void Turn_Off_MuzzleFlash()
    {
        muzzleFlash.SetActive(false);
    }

    // sound
    public void Play_ShootSound()
    {
        shoot_sound.Play();
    }
    public void Play_ReloadSound()
    {
        reload_sound.Play();
    }

    // attack point
    public void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }
    public void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

}
