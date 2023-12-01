using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySlimeController : MonoBehaviour
{
    public int HP = 10;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float detectionDistance = 6f;
    public float attackDistance = 2f;
    public int damage = 5;
    public NavMeshAgent _nav;
    public GameObject meleeCollider;
    [Range(0, 5)] public float FootstepVolume = 2f;
    public AudioClip FootstepSound;
    public GameObject HealthBar;
    public float lerp = 0.01f;
    public int HealthBarHeight = 150;

    [Range(0, 1)] public float AudioVolume = 0.75f;
    public AudioClip AttackSound;
    public AudioClip OnHitSound;
    public AudioClip DeathSound;

    private Vector3 _playerPosition;
    private GameObject _player;
    private NewPlayerController _playerController;
    private Animator _animator;
    private bool _alive = true;
    private float _distanceToPlayer;
    private Slider _healthBarSlider;
    private float _damageTimeout = 0f;
    private float _timeout = 0f;
    private bool _engaged = false;

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
        HealthBar.transform.position = new Vector3(position.x, position.y+HealthBarHeight, position.z);
        HealthBar.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _damageTimeout -= Time.deltaTime;
        _timeout -= Time.deltaTime;
        _distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);

        if (_alive)
        {
            DetectPlayer();
        }
        SetHeath();
    }

    void DetectPlayer()
    {
        if (_timeout <= 0)
        {
            if (SeesPlayer())
            {
                _engaged = true;
                if (_distanceToPlayer <= attackDistance)
                {
                    AttackPlayer();
                }
                else
                {
                    ChasePlayer();
                }
            }
            else if (_engaged)
            {
                if (_distanceToPlayer > attackDistance && _distanceToPlayer <= detectionDistance)
                {
                    ChasePlayer();
                }
                else if (_distanceToPlayer > detectionDistance)
                {
                    SetIdle();
                    _engaged = false;
                }
            }

            _timeout = 0.2f;
        }
        
    }

    void ChasePlayer()
    {
        HealthBar.SetActive(true);
        _animator.SetBool("chasePlayer", true);
        _animator.SetBool("attackPlayer", false);
        _nav.isStopped = false;
        _nav.SetDestination(_player.transform.position);
    }

    void AttackPlayer()
    {
        HealthBar.SetActive(true);
        _animator.SetBool("chasePlayer", false);
        _animator.SetBool("attackPlayer", true);
        _nav.isStopped = true;
        _nav.velocity = Vector3.zero;
        transform.LookAt(_player.transform);
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
    }

    void SetIdle()
    {
        HealthBar.SetActive(false);
        _animator.SetBool("chasePlayer", false);
        _animator.SetBool("attackPlayer", false);
        _nav.isStopped = true;
    }

    void MeleeDamage()
    {
        meleeCollider.SetActive(true);
        AudioSource.PlayClipAtPoint(AttackSound, transform.position, AudioVolume);
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
                    // turn to look at player
                    transform.LookAt(_player.transform);
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

    void OnBounce()
    {
        AudioSource.PlayClipAtPoint(FootstepSound, transform.position, AudioVolume);
    }

    void SetHeath()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 healthBarPosition = new Vector3(position.x, position.y+HealthBarHeight, position.z);
        HealthBar.transform.position = Vector3.Lerp(HealthBar.transform.position, healthBarPosition, lerp);
        _healthBarSlider.value = HP;
    }

    bool SeesPlayer()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
