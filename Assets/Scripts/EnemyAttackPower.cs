using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPower : MonoBehaviour
{
    public int MaxDamage = 5;
    public int Damage = 5;

    private float _damageTimeout = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _damageTimeout -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_damageTimeout <= 0)
        {
            if (other.gameObject.CompareTag("Shield"))
            {
                Damage = 0;
                _damageTimeout = 0.5f;
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                Damage = MaxDamage;
                _damageTimeout = 0.5f;
            }
        }
    }
}
