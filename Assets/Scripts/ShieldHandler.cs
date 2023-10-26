using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHandler : MonoBehaviour
{
    public AudioClip ShieldBlockSound;
    [Range(0, 1)] public float AudioVolume = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyProjectile") || other.gameObject.CompareTag("EnemyMelee"))
        {
            AudioSource.PlayClipAtPoint(ShieldBlockSound, transform.position, AudioVolume);
        }
    }
}