using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlantController : MonoBehaviour
{
    [SerializeField] private float timer = 5;
    private float bulletTime;

    public int HP = 15;
    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float bulletSpeed = 100;
    public float detectionDistance = 5f;
    [Range(0, 1)] public float AudioVolume = 0.75f;
    public AudioClip BulletSound;
    public AudioClip OnHitSound;
    public AudioClip DeathSound;

    private Vector3 _playerPosition;
    private GameObject _player;
    private NewPlayerController _playerController;
    private Animator _animator;
    private bool _alive = true;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<NewPlayerController>();
        _animator = GetComponent<Animator>();
        bulletTime = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_alive)
        {
            DetectPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon")) 
        {
            if (_playerController._attacking > 0)
            {
                AudioSource.PlayClipAtPoint(OnHitSound, transform.position, AudioVolume);
                // take damage
                HP -= _playerController._attacking;
            }
        }

        if (HP <= 0)
        {
            _alive = false;
            _animator.Play("Die");
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
        if (bulletTime < 1) _animator.Play("ShootBullet");
        if (bulletTime > 0) return;
        bulletTime = timer;
        
        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();

        Vector3 _playerPosition = _player.transform.position;

        bulletRig.AddForce(bulletRig.transform.forward * bulletSpeed);
        Destroy(bulletObj, 5f);
    }

    void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    void OnShootBullet()
    {
        AudioSource.PlayClipAtPoint(BulletSound, transform.position, AudioVolume);
    }

    void OnDeath()
    {
        AudioSource.PlayClipAtPoint(DeathSound, transform.position, AudioVolume);
    }
}