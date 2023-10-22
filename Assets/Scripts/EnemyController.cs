using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float timer = 5;
    private float bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float bulletSpeed = 100;
    public float detectionDistance = 5f;

    private Vector3 _playerPosition;
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        bulletTime = timer;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("Player")) {
        //     transform.LookAt(_player);
        //     ShootAtPlayer();
        // }

        if (other.gameObject.CompareTag("Weapon")) 
        {
            Destroy(this.gameObject);
        }
    }

    void DetectPlayer()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) <= detectionDistance)
        {
            transform.LookAt(_player.transform);
            ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;
        if (bulletTime > 0) return;
        bulletTime = timer;
        
        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();

        Vector3 _playerPosition = _player.transform.position;

        bulletRig.AddForce(bulletRig.transform.forward * bulletSpeed);
        Destroy(bulletObj, 5f);
    }
}

} // namespace StarterAssets
