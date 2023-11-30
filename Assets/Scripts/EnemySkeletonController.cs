using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySkeletonController : MonoBehaviour
{
    public int HP = 50;
    public GameObject HealthBar;
    public float detectionDistance = 6f;
    public float attackDistance = 2f;
    public int damage = 5;
    public NavMeshAgent _nav;
    public GameObject meleeCollider;
    public float lerp = 0.01f;

    [Range(0, 5)] public float AudioVolume = 1f;
    public AudioClip VerticalAxeSound;
    public AudioClip HorizontalAxeSound;
    public AudioClip OnHitSound;
    public AudioClip DeathSound;
    public AudioClip FootstepSound;

    private Vector3 _playerPosition;
    private GameObject _player;
    private NewPlayerController _playerController;
    private Animator _animator;
    private bool _alive = true;
    private float _distanceToPlayer;
    private Slider _healthBarSlider;
    private float _damageTimeout = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<NewPlayerController>();
        _distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
        _animator = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        meleeCollider.SetActive(false);
        
        _healthBarSlider = HealthBar.GetComponent<Slider>();
        _healthBarSlider.maxValue = HP;
        _healthBarSlider.value = HP;
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        HealthBar.transform.position = new Vector3(position.x, position.y+300, position.z);
        HealthBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _damageTimeout -= Time.deltaTime;
        _distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);

        if (_alive)
        {
            DetectPlayer();
        }
        SetHeath();
    }

    void DetectPlayer()
    {
        if (_distanceToPlayer <= attackDistance)
        {
            HealthBar.SetActive(true);
            AttackPlayer();
        }
        else if (_distanceToPlayer <= detectionDistance)
        {
            HealthBar.SetActive(true);
            ChasePlayer();
        }
        else
        {
            HealthBar.SetActive(false);
        }
    }

    void ChasePlayer()
    {
        _animator.SetBool("chasePlayer", true);
        _animator.SetBool("attackPlayer", false);
        _nav.isStopped = false;
        _nav.SetDestination(_player.transform.position);
    }

    void AttackPlayer()
    {
        _animator.SetBool("chasePlayer", false);
        _animator.SetBool("attackPlayer", true);
        _nav.isStopped = true;
        _nav.velocity = Vector3.zero;
        transform.LookAt(_player.transform);
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
    }

    void MeleeDamage()
    {
        meleeCollider.SetActive(true);
    }

    void EndMeleeDamage()
    {
        meleeCollider.SetActive(false);
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
                    // stagger enemey if enough damage
                    if (_playerController._attacking >= 10)
                    {
                        _animator.Play("Stagger");
                    }
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

    void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    void OnDeath()
    {
        AudioSource.PlayClipAtPoint(DeathSound, transform.position, AudioVolume);
    }

    void OnVerticalAxe()
    {
        AudioSource.PlayClipAtPoint(VerticalAxeSound, transform.position, AudioVolume);
    }

    void OnHorizontalAxe()
    {
        AudioSource.PlayClipAtPoint(HorizontalAxeSound, transform.position, AudioVolume);
    }

    void OnFootstep()
    {
        AudioSource.PlayClipAtPoint(FootstepSound, transform.position, AudioVolume);
    }

    void SetHeath()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 healthBarPosition = new Vector3(position.x, position.y+300, position.z);
        HealthBar.transform.position = Vector3.Lerp(HealthBar.transform.position, healthBarPosition, lerp);
        _healthBarSlider.value = HP;
    }
}
