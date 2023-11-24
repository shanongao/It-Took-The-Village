using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDefense : MonoBehaviour
{
    [Tooltip("When blocking, any enemy attack with power less than defense will be negated")]
    public int defense = 5;
    [Tooltip("If enemy attack power is greater than defense, reduce damage by block rate")]
    public float blockRate = 0.25f;
    public int maxDefense = 15;
    public int upgradeIncrement = 5;

    bool UpgradeShield()
    {
        if (defense < maxDefense)
        {
            defense += upgradeIncrement;
            blockRate += 0.25f;
            if (defense > maxDefense)
            {
                defense = maxDefense;
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
