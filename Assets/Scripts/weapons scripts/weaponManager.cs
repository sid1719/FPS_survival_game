using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponManager : MonoBehaviour
{   [SerializeField] 
    private WeaponHandler[] weapons;

    private int current_weapon_Index;
    void Start()
    {
        current_weapon_Index = 0;
        weapons[current_weapon_Index].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOnSelectedWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOnSelectedWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TurnOnSelectedWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TurnOnSelectedWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TurnOnSelectedWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TurnOnSelectedWeapon(5);
        }
    }

    void TurnOnSelectedWeapon(int weaponIndex)
    {
        weapons[current_weapon_Index].gameObject.SetActive(false);

        weapons[weaponIndex].gameObject.SetActive(true);

        current_weapon_Index = weaponIndex;
    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
        return weapons[current_weapon_Index];

    }
}
