using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BossEnemyController : MonoBehaviour
{
    [SerializeField] private float timer = 5;
    [SerializeField] private string sceneName;
    private float bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float bulletSpeed = 100;
    public float detectionDistance = 5f;
    public int HP = 50;
    public GameObject BossUI;
    public string BossName;

    [Range(0, 1)] public float AudioVolume = 0.75f;
    public AudioClip BulletSound;
    public AudioClip OnHitSound;
    public AudioClip DeathSound;
    
    private GameObject _player;
    private NewPlayerController _playerController;
    private Animator _animator;
    private bool _alive = true;

    void Start()
    {
        BossUI.SetActive(false);
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<NewPlayerController>();
        _animator = GetComponent<Animator>();
        bulletTime = timer;
    }

    void Update()
    {
        if (_alive)
        {
            DetectPlayer();
        }
    }

    void DetectPlayer()
    {
        float distance = Vector3.Distance(_player.transform.position, transform.position);
        if (distance <= detectionDistance)
        {
            BossUI.SetActive(true);
            TextMeshProUGUI tmp = BossUI.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            tmp.SetText(BossName);
            transform.LookAt(_player.transform);
            ShootAtPlayer();
        }
        else
        {
            BossUI.SetActive(false);
        }
    }

    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;
        if (bulletTime < 1) _animator.Play("Attack");
        if (bulletTime > 0) return;
        bulletTime = timer;

        int numRockets = 12;  
        float angleStep = 360f / numRockets;

        for (int i = 0; i < numRockets; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 adjustedSpawnPosition = spawnPoint.transform.position + new Vector3(0, 0.5f, 0);
            GameObject rocketObj = Instantiate(enemyBullet, adjustedSpawnPosition, rotation) as GameObject;
            Vector3 forwardDirection = rotation * Vector3.forward;
            rocketObj.transform.Rotate(Vector3.right * 90, Space.Self);
            Rigidbody rocketRig = rocketObj.GetComponent<Rigidbody>();
            rocketRig.AddForce(forwardDirection * bulletSpeed);
            Destroy(rocketObj, 5f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon")) 
        {
            if (_playerController._attacking)
            {
                HP -= 5; // @TODO hard code damage point for now
                AudioSource.PlayClipAtPoint(OnHitSound, transform.position, AudioVolume);
                if (HP <= 0)
                {
                    _alive = false;
                    _animator.Play("Die");
                    BossUI.SetActive(false);
                }
            }
        }
    }

    void DestroyEnemy()
    {
        Destroy(this.gameObject);
        SceneManager.LoadScene(sceneName);
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
