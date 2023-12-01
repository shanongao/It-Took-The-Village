using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackPower : MonoBehaviour
{
    public int attackPower = 5;
    public int maxAttackPower = 15;
    public int upgradeIncrement = 5;

    public bool UpgradeWeapon()
    {
        if (attackPower < maxAttackPower)
        {
            attackPower = Mathf.RoundToInt(attackPower * 1.8f);
            if (attackPower > maxAttackPower)
            {
                attackPower = maxAttackPower;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
