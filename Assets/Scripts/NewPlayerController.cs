using UnityEngine;
using TMPro;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class NewPlayerController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the camera turns")]
        public float CameraRotationSpeed = 10.0f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public GameObject Inventory;
        [Tooltip("Location to equip sword")]
        public GameObject SwordHolder;
        [Tooltip("Location to equip ax")]
        public GameObject AxeHolder;
        [Tooltip("Location to equip hammer")]
        public GameObject HammerHolder;
        [Tooltip("Location to equip shield")]
        public GameObject ShieldHolder;

        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        public AudioClip[] RunningFootstepAudioClips;
        [Range(0, 1)] public float AudioVolume = 0.75f;
        public AudioClip SwordSlashSound;
        public AudioClip DamageSound;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        public GameObject DefaultCamera;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        public float rootMovementSpeed = 1f;
        public float rootTurnSpeed = 1f;
        
        //UI
        public TextMeshProUGUI countText;
        public GameObject GameOverScreen;
        public int maxHealth = 100;
        public int currentHealth;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private Vector3 _relativeCameraPosition;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        public bool _blocking = false;
        private float _damageTimeout = 0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        private PlayerInput _playerInput;

        private Animator _animator;
        private Rigidbody _rbody;
        private CharacterController _controller;
        private GameObject _mainCamera;

        private Vector2 _moveVector;
        private Vector2 _look;
        private bool _sprint;
        private bool _jump;
        private bool _snap = false;
        private BoxCollider _shieldCollider;

        private equipWeapon _inventory;
        public int _equippedWeapon = -1;

        private const float _threshold = 0.01f;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void InitWeapons()
        {
            if (HammerHolder.transform.GetChild(0).gameObject.activeSelf)
            {
                _equippedWeapon = 0;
                _inventory.activeWeapons[0] = true;
            }
            else if (SwordHolder.transform.GetChild(0).gameObject.activeSelf)
            {
                _equippedWeapon = 1;
                _inventory.activeWeapons[1] = true;
            }
            else if (AxeHolder.transform.GetChild(0).gameObject.activeSelf)
            {
                _equippedWeapon = 2;
                _inventory.activeWeapons[2] = true;
            }
        }

        private void Start()
        {
            currentHealth = maxHealth;
            SetHP();
            GameOverScreen.SetActive(false);

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _relativeCameraPosition = transform.InverseTransformPoint(CinemachineCameraTarget.transform.position);
            
            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _rbody = GetComponent<Rigidbody>();
            _shieldCollider = ShieldHolder.GetComponent<BoxCollider>();
            _inventory = Inventory.GetComponent<equipWeapon>();
            InitWeapons();

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {

            if (GameOverScreen.activeSelf)
            {
                _playerInput.enabled = false;
            }

            JumpAndGravity();
            GroundedCheck();
            Move();
            Action();
            SetHP();
            OnEquip();
        }

        private void LateUpdate()
        {
            CameraRotation();
            if (_snap)
            {
                CinemachineCameraTarget.transform.position = DefaultCamera.transform.position;
                CinemachineCameraTarget.transform.rotation = DefaultCamera.transform.rotation;
                _snap = false;
            }
            _damageTimeout -= Time.deltaTime;
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            _animator.SetBool(_animIDGrounded, Grounded);
        }

        private void CameraRotation()
        {
            // do not move the camera if game is paused
            if (Time.timeScale == 0)
            {
                return;
            }

            // if there is an input and camera position is not fixed
            if (_look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = CameraRotationSpeed;

                _cinemachineTargetYaw += _look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
            // CinemachineCameraTarget.transform.RotateAround(transform.position, Vector3.up, _look.x);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveVector = context.ReadValue<Vector2>();
            if (_moveVector.magnitude > 0)
            {
                _animator.SetBool("isWalking", true);
            }
            else
            {
                _animator.SetBool("isWalking", false);
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
		{
			_sprint = context.ReadValueAsButton();
            if (_sprint && _animator.GetBool("isWalking"))
            {
                _animator.SetBool("isRunning", true);
            }
            else if (!_sprint)
            {
                _animator.SetBool("isRunning", false);
            }
		}

        public void OnLook(InputAction.CallbackContext context)
		{
			_look = context.ReadValue<Vector2>();
		}

        public void OnSnapCamera(InputAction.CallbackContext context)
		{	
            if (context.performed)
            {
                _snap = true;
            }
		}

        public void OnJump(InputAction.CallbackContext context)
        {
            _jump = context.ReadValueAsButton();
            _animator.Play("Jump");
        }

        public void OnBlock(InputAction.CallbackContext context)
        {
            _blocking = context.ReadValueAsButton();
            if (_blocking && ShieldHolder.transform.GetChild(0).gameObject.activeSelf)
            {
                _animator.SetBool("isBlocking", true);
            }
            else
            {
                _animator.SetBool("isBlocking", false);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed && _equippedWeapon >= 0)
            {
                if (_animator.GetBool("isWalking"))
                {
                    _animator.Play("DownSwingMoving");
                }
                else
                {
                    _animator.Play("DownSwing");
                }
            }
        }

        public void OnEquipSword(InputAction.CallbackContext context)
        {
            if (context.performed && _inventory.activeWeapons[1])
            {
                _equippedWeapon = 1;
            }
        }

        public void OnEquipHammer(InputAction.CallbackContext context)
        {
            if (context.performed && _inventory.activeWeapons[0])
            {
                _equippedWeapon = 0;
            }
        }

        public void OnEquipAxe(InputAction.CallbackContext context)
        {
            if (context.performed && _inventory.activeWeapons[2])
            {
                _equippedWeapon = 2;
            }
        }

        private void OnEquip()
        {
            if (_equippedWeapon == 0 && _inventory.activeWeapons[0])
            {
                SwordHolder.transform.GetChild(0).gameObject.SetActive(false);
                HammerHolder.transform.GetChild(0).gameObject.SetActive(true);
                AxeHolder.transform.GetChild(0).gameObject.SetActive(false);
            }
            else if (_equippedWeapon == 1 && _inventory.activeWeapons[1])
            {
                SwordHolder.transform.GetChild(0).gameObject.SetActive(true);
                HammerHolder.transform.GetChild(0).gameObject.SetActive(false);
                AxeHolder.transform.GetChild(0).gameObject.SetActive(false);
            }
            else if (_equippedWeapon == 2 && _inventory.activeWeapons[2])
            {
                SwordHolder.transform.GetChild(0).gameObject.SetActive(false);
                HammerHolder.transform.GetChild(0).gameObject.SetActive(false);
                AxeHolder.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_moveVector == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            // float speedOffset = 0.1f;
            float inputMagnitude = 1f;

            // accelerate or decelerate to target speed
            // if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            //     currentHorizontalSpeed > targetSpeed + speedOffset)
            // {
            //     // creates curved result rather than a linear one giving a more organic speed change
            //     // note T in Lerp is clamped, so we don't need to clamp our speed
            //     _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
            //         Time.deltaTime * SpeedChangeRate);

            //     // round speed to 3 decimal places
            //     _speed = Mathf.Round(_speed * 1000f) / 1000f;
            // }
            // else
            // {
            //     _speed = targetSpeed;
            // }
            _speed = targetSpeed;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_moveVector.x, 0.0f, _moveVector.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_moveVector != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            // _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDSpeed, _speed);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);

            GameObject temp = new GameObject();
            temp.transform.position = transform.position;
            temp.transform.rotation = CinemachineCameraTarget.transform.rotation;
            CinemachineCameraTarget.transform.position = temp.transform.TransformPoint(_relativeCameraPosition);
        }

        private void Action()
        {
            
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                // if (_jump && _jumpTimeoutDelta <= 0.0f)
                // {
                //     // the square root of H * -2 * G = how much velocity needed to reach desired height
                //     _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                //     // update animator if using character
                //     _animator.SetBool(_animIDJump, true);
                //     _animator.Play("Jump");
                // }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    _animator.SetBool(_animIDFreeFall, true);
                }

                // if we are not grounded, do not jump
                _jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnRunningFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (RunningFootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, RunningFootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(RunningFootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume*1.2f);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            int damage = 0;
            if (other.gameObject.CompareTag("EnemyProjectile"))
            {
                EnemyAttackPower attack = other.gameObject.GetComponent<EnemyAttackPower>();
                damage = attack.Damage;
                Destroy(other.gameObject);
            }

            if (other.gameObject.CompareTag("EnemyMelee"))
            {
                EnemyAttackPower attack = other.gameObject.GetComponent<EnemyAttackPower>();
                damage = attack.Damage;
            }

            if (_damageTimeout <= 0)
            {
                if (damage > 0)
                {
                    AudioSource.PlayClipAtPoint(DamageSound, transform.TransformPoint(_controller.center), AudioVolume);
                }
                TakeDamage(damage);
                _damageTimeout = 0.5f;
            }
        }

        void TakeDamage(int damage)
        {
            currentHealth -= damage;
        }

        void SetHP()
        {
            if (currentHealth <= 0)
            {
                GameOverScreen.SetActive(true);
                _animator.Play("Die");
            }
            countText.text = "HP: " + currentHealth.ToString();
        }

        void ShieldUp()
        {
            _shieldCollider.enabled = true;
            _blocking = true;
        }

        void ShieldDown()
        {
            _shieldCollider.enabled = false;
            _blocking = false;
        }

        void OnSwordSlash()
        {
            AudioSource.PlayClipAtPoint(SwordSlashSound, transform.TransformPoint(_controller.center), AudioVolume);
        }
    }
