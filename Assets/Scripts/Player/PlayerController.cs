using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Global Variables
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private Collider2D _hitbox;

    [Header("Movement settings")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _airAcceleration;

    [Header("Jump settings")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _maxJumps;
    [SerializeField] private float _bufferTime;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _maxFallSpeed;
    [SerializeField] private float _coyoteTime;

    [Header("Dash Settings")]
    [SerializeField] private float _dashCd;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;

    [Header("VFX")]
    [SerializeField] private GameObject _interactionIndicator;
    [SerializeField] private Sprite[] _interactionSprites;

    [Header("Audio")]
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _movementSound;

    [Header("Audio Volumes")]
    [SerializeField] private float _movementVolume = 1.0f;
    [SerializeField] private float _jumpVolume = 1.0f;

    public bool DesiredInteraction { get; set; }

    private PlayerCombat _playerCombat;
    private Rigidbody2D _rigidbody2D;
    private Animator _myAnimator;
    private bool _isDead;

    // Movement
    private float _movementInput;
    private Vector2 _desiredVelocity;
    private bool _isGrounded;
    private bool _isOverride;

    // Jump
    private int _availableJumps;
    private float _jumpPressedTime;
    private float _jumpPerformedTime;
    private bool _stopJump;
    private float _coyoteStart;
    private bool _desiredJump;
    private float _timer;

    // Dash
    private bool _desiredDash;
    private float _dashPerformedTime;
    private bool _isDashing;
    private float _dashDirection;

    private AudioSource _audioSource;
    #endregion

    #region Unity methods
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerCombat = GetComponent<PlayerCombat>();
        _myAnimator = GetComponent<Animator>();

        if (_groundCheck == null) Debug.LogError("[PlayerController] La refer�ncia a Ground Check �s null");

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (LevelProgressController.Instance.HasSpawnPoint)
        {
            transform.position = LevelProgressController.Instance.SpawnPoint;
        }
    }

    void Update()
    {
        HandleInputs();
        _timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
        Dash();
        HandleEnvironment();
        Jump();
        ApplyMovement();
    }
    #endregion

    #region Inputs
    /// <summary>
    /// Function that gets the player movement inputs
    /// </summary>
    private void HandleInputs()
    {
        if (_isOverride) return;

        if (!_playerCombat.IsAttacking && !_playerCombat.IsParrying)
        {
            _movementInput = PlayerInputsManager.Instance.ReadHorizontalInput();
            
        }
        else
        {
            _movementInput = 0;
        }

        if (_movementInput != 0 && _isGrounded)
        {
            if (_movementSound != null && !_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(_movementSound, _movementVolume);
            }
        }
    }

    /// <summary>
    /// Function called when the player dies
    /// </summary>
    public void Die()
    {
        PlayerInputsManager.Instance.DisableControls();
    }
    #endregion

    #region Movement
    /// <summary>
    /// Function that handles with the external conditions that affect the player movement
    /// </summary>
    private void HandleEnvironment()
    {
        //Només entra si el jugador es deixa caure, per diferenciar de si ha saltat
        if (!Physics2D.OverlapCircle(_groundCheck.transform.position, 0.05f, LayerMask.GetMask("Ground")) && _isGrounded)
        {
            _coyoteStart = _timer;
            _availableJumps -= 1;
        }

        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, 0.1f, LayerMask.GetMask("Ground"));

        _myAnimator.SetBool("isGrounded", _isGrounded);

        float raycastOffset = 1.3f;

        // Raycast to check if the head is colliding with an obstacle
        RaycastHit2D rightRaycast = Physics2D.Raycast(transform.position + new Vector3(0.1f, raycastOffset), Vector2.up, 0.5f, LayerMask.GetMask("Roof"));
        Debug.DrawRay(transform.position + new Vector3(0.4f, raycastOffset), Vector2.up * 0.5f, Color.green);
        RaycastHit2D centerRaycast = Physics2D.Raycast(transform.position + new Vector3(-0.1f, raycastOffset), Vector2.up, 0.5f, LayerMask.GetMask("Roof"));
        Debug.DrawRay(transform.position + new Vector3(0, raycastOffset), Vector2.up * 0.5f );
        RaycastHit2D leftRaycast = Physics2D.Raycast(transform.position + new Vector3(-0.3f, raycastOffset), Vector2.up, 0.5f, LayerMask.GetMask("Roof"));
        Debug.DrawRay(transform.position + new Vector3(-0.4f, raycastOffset), Vector2.up * 0.5f, Color.red);

        // Moves the player if its partially colliding
        if (rightRaycast && !centerRaycast)
        {
            transform.position -= new Vector3 (0.3f, 0);
        }     
        else if (leftRaycast && !centerRaycast)
        {
            transform.position += new Vector3(0.3f, 0);
        }

        if (centerRaycast) _stopJump = true;       
        
    }

    /// <summary>
    /// Function that handles the horizontal movement of the player
    /// </summary>
    public void HorizontalMovement()
    {
        if (_isDashing)
        {
            if (_timer - _dashPerformedTime < _dashDuration)
            {
                _desiredVelocity.x = _dashSpeed * _dashDirection;
            }
            else
            {
                // Ends the dash
                _isDashing = false;
                _desiredVelocity.x = _maxSpeed * _dashDirection;
                _hitbox.enabled = true;
                _myAnimator.SetBool("isDashing", false);
            }
        }
        else
        {
            if (_movementInput == 0)
            {
                _desiredVelocity.x = Mathf.MoveTowards(_desiredVelocity.x, 0, _acceleration * Time.fixedDeltaTime);
            }
            else
            {
                float acceleration = _isGrounded ? _acceleration : _airAcceleration;
                _desiredVelocity.x = Mathf.MoveTowards(_desiredVelocity.x, _movementInput * _maxSpeed, acceleration * Time.fixedDeltaTime);
                if (_movementInput != 0 && _isGrounded)
                {
                    if (_movementSound != null && !_audioSource.isPlaying)
                    {
                        _audioSource.PlayOneShot(_movementSound);
                    }
                }
            }
        }
        if (_desiredVelocity.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            _interactionIndicator.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (_desiredVelocity.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _interactionIndicator.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        _myAnimator.SetFloat("horizontalVelocity", Mathf.Abs(_desiredVelocity.x));
    }

    private void Dash()
    {
        if (_desiredDash && !_playerCombat.IsAttacking && !_playerCombat.IsParrying)
        {
            _desiredDash = false;

            // If it can, dashes towards the direction the player is inputting
            if (_timer - _dashPerformedTime > _dashCd)
            {
                _dashPerformedTime = _timer;
                _isDashing = true;

                float inputDirection = PlayerInputsManager.Instance.ReadHorizontalInput();

                if (inputDirection == 0) _dashDirection = transform.right.x;
                else _dashDirection = inputDirection;

                _hitbox.enabled = false;

                _myAnimator.SetBool("isDashing", true);
            }
        }
    }

    public void HandleDashInput()
    {
        _desiredDash = true;
    }
    #endregion

    #region Jump
    /// <summary>
    /// Function that handles the executrion of the jump considering all the variables that are implied
    /// </summary>
    public void Jump()
    {
        if (_isGrounded)
        {
            _availableJumps = _maxJumps;
            _stopJump = false;
        }

        bool bufferedJump = _timer - _jumpPressedTime <= _bufferTime;
        bool canJump = (_availableJumps > 0 || _timer - _coyoteStart <= _coyoteTime) && !_playerCombat.IsAttacking && !_playerCombat.IsParrying && !_isDashing;

        // Limits the gravity when the player is grounded
        if (_isGrounded && _desiredVelocity.y <= 0) _desiredVelocity.y = -0.1f;
        else
        {
            if (_stopJump) _desiredVelocity.y = Mathf.Min(0, _desiredVelocity.y);
            _desiredVelocity.y = Mathf.MoveTowards(_desiredVelocity.y, -_maxFallSpeed, _fallSpeed * Time.fixedDeltaTime);
        }

        // Jumps if the conditions are met
        if (bufferedJump && canJump && _desiredJump && _timer - _jumpPerformedTime > 0.2f)
        {
            _jumpPerformedTime = _timer;
            _desiredVelocity.y = _jumpForce;
            _desiredJump = false;
            _availableJumps -= 1;
            if (_jumpSound != null)
            {
                _audioSource.PlayOneShot(_jumpSound);
            }
        }
  
    }

    /// <summary>
    /// Function that applies the final movement to the player
    /// </summary>
    public void ApplyMovement()
    {
        if (_isOverride) return;

        if (_playerCombat.IsAttacking || _isDashing) _desiredVelocity.y = 0;
        _rigidbody2D.velocity = _desiredVelocity;
    }

    /// <summary>
    /// Function that executes then the jump button is pressed and lets the script know
    /// </summary>
    public void HandleJumpInput()
    {
        _jumpPressedTime = _timer;
        _desiredJump = true;
    }
    #endregion
    
    #region Inventory
    /// <summary>
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="item">Item to add</param>
    public void AddItem(InventoryItem item)
    {
        _inventoryController.AddItem(item);
    }

    public bool TryGetItem(InventoryItem.ItemType type, out InventoryItem outItem)
    {
        if (_inventoryController.TryGetItem(type, out InventoryItem item))
        {
            outItem = item;
            return true;
        }

        outItem = null;
        return false;
    }

    /// <summary>
    /// Checks if the given object is in the inventory
    /// </summary>
    /// <param name="type">The type of the object</param>
    /// <param name="id">The id of the object</param>
    /// <returns>Whether the object is in the inventory or not</returns>
    public bool HasItem(InventoryItem.ItemType type, int id = -1)
    {
        if (id == -1)
            return _inventoryController.HasItem(type);
        else
            return _inventoryController.HasKey(id);
    }

    /// <summary>
    /// Removes an object from the inventory
    /// </summary>
    /// <param name="type">The type of the object</param>
    /// <param name="id">The id of the object</param>
    public void RemoveItem(InventoryItem.ItemType type, int id = -1)
    {
        if (id == -1)
            _inventoryController.RemoveItem(type);
        else
            _inventoryController.RemoveKey(id);
    }
    #endregion

    #region UI
    public void CanInteract(bool can)
    {
        if (can)
        {
            switch (PlayerInputsManager.Instance.InputDevice)
            {
                case PlayerInputsManager.InputDevices.Keyboard:
                    _interactionIndicator.GetComponent<SpriteRenderer>().sprite = _interactionSprites[0];
                    break;

                case PlayerInputsManager.InputDevices.Controller:
                    _interactionIndicator.GetComponent<SpriteRenderer>().sprite = _interactionSprites[1];
                    break;
            }
            _interactionIndicator.SetActive(true);
        }
        else
        {
            _interactionIndicator.SetActive(false);
        }
    }
    #endregion

    #region Getters / Setters
    public Rigidbody2D Rigidbody => _rigidbody2D;
    public bool IsOverride { get { return _isOverride; } set { _isOverride = value; } }
    public bool IsGrounded => _isGrounded;
    public bool IsDead => _isDead;
    public bool IsDashing => _isDashing;
    #endregion
}
