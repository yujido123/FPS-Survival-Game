using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField]
    private WeaponHandler[] weaponHandlers;

    // current used weapon
    private int currentWeaponIndex;

    void Start()
    {
        currentWeaponIndex = -1;
        TurnOnWeapon(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOnWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOnWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TurnOnWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TurnOnWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TurnOnWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TurnOnWeapon(5);
        }
    }

    void TurnOnWeapon(int weaponIndex)
    { 
        if(weaponIndex == currentWeaponIndex)
            return;

        if(currentWeaponIndex >= 0)
            weaponHandlers[currentWeaponIndex].gameObject.SetActive(false);

        weaponHandlers[weaponIndex].gameObject.SetActive(true);
        currentWeaponIndex = weaponIndex;

    }

    public WeaponHandler GetCurrentWeapon()
    {
        return weaponHandlers[currentWeaponIndex];
    }
}
