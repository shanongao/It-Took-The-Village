using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySlimeController : MonoBehaviour
{
    public float detectionDistance = 6f;
    public float attackDistance = 2f;
    public int damage = 5;
    public NavMeshAgent _nav;
    public GameObject meleeCollider;
    [Range(0, 5)] public float FootstepVolume = 2f;
    public AudioClip FootstepSound;

    [Range(0, 1)] public float AudioVolume = 0.75f;
    public AudioClip AttackSound;
    public AudioClip OnHitSound;
    public AudioClip DeathSound;

    private Vector3 _playerPosition;
    private GameObject _player;
    private Animator _animator;
    private bool _alive = true;
    private float _distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
        _animator = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        meleeCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);

        if (_alive)
        {
            DetectPlayer();
        }
    }

    void DetectPlayer()
    {
        if (_distanceToPlayer <= attackDistance)
        {
            AttackPlayer();
        }
        else if (_distanceToPlayer <= detectionDistance)
        {
            ChasePlayer();
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
        if (other.gameObject.CompareTag("Weapon")) 
        {
            Animator playerAnimator = _player.GetComponent<Animator>();
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("DownSwing") || 
                playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("DownSwingMoving"))
            {
                AudioSource.PlayClipAtPoint(OnHitSound, transform.position, AudioVolume);
                _alive = false;
                _animator.Play("Die");
            }
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
}
