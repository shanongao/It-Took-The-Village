using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject HealthBar;
    public float lerp = 0.01f;
    public int HealthBarHeight = 150;

    private Vector3 _playerPosition;
    private GameObject _player;
    private NewPlayerController _playerController;
    private Animator _animator;
    private bool _alive = true;
    private Slider _healthBarSlider;
    private float _damageTimeout = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<NewPlayerController>();
        _animator = GetComponent<Animator>();
        bulletTime = timer;

        _healthBarSlider = HealthBar.GetComponent<Slider>();
        _healthBarSlider.maxValue = HP;
        _healthBarSlider.value = HP;
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        HealthBar.transform.position = new Vector3(position.x, position.y+HealthBarHeight, position.z);
        HealthBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _damageTimeout -= Time.deltaTime;
        if (_alive)
        {
            DetectPlayer();
        }
        SetHeath();
    }

    void OnTriggerEnter(Collider other)
    {
        if (_damageTimeout <= 0)
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
            _damageTimeout = 0.1f;
        }
    }

    void DetectPlayer()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) <= detectionDistance)
        {
            HealthBar.SetActive(true);
            transform.LookAt(_player.transform);
            ShootAtPlayer();
        }
        else if (Vector3.Distance(_player.transform.position, transform.position) > detectionDistance*2)
        {
            HealthBar.SetActive(false);
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

    void SetHeath()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 healthBarPosition = new Vector3(position.x, position.y+HealthBarHeight, position.z);
        HealthBar.transform.position = Vector3.Lerp(HealthBar.transform.position, healthBarPosition, lerp);
        _healthBarSlider.value = HP;
    }
}