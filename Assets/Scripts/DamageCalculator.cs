using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(5);
        }
    }

    void TakeDamage(int damage)
    {
        player.currentHealth -= damage;
    }
}
