using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPower : MonoBehaviour
{
    public int MaxDamage = 5;
    public int Damage = 5;

    private float _damageTimeout = 0f;
    private NewPlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<NewPlayerController>();
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
            if (other.gameObject.CompareTag("Shield") && _playerController._blocking)
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
