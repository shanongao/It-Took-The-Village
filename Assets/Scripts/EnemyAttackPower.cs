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
                Debug.Log("Blocked");
                GameObject shield = other.gameObject;
                int defense = shield.GetComponent<ShieldDefense>().defense;
                float blockRate = shield.GetComponent<ShieldDefense>().blockRate;
                if (Damage > defense)
                {
                    Damage = Mathf.RoundToInt(MaxDamage * blockRate);
                }
                else
                {
                    Damage = 0;
                }

                Animator animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
                animator.Play("BlockImpact");
                _damageTimeout = 0.1f;
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                Damage = MaxDamage;
                _damageTimeout = 0.1f;
            }
        }
    }
}
