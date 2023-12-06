using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject _groundCheck;
    [SerializeField] InventoryController _inventoryController;

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

    private PlayerCombat _playerCombat;
    private PlayerInputActions _playerInputActions;
    private Rigidbody2D _rigidbody2D;
    private int _availableJumps;
    private float _movementInput;
    private Vector2 _desiredVelocity;
    private bool _isGrounded;
    private float _jumpPressed;
    private float _timer;
    private bool _stopJump;
    private float _coyoteStart;
    private bool _desiredJump;
    private bool _jumpHold;

    private Animator _myAnimator;
    private bool _isOverride;
    #endregion

    #region Unity methods
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerCombat = GetComponent<PlayerCombat>();
        _myAnimator = GetComponent<Animator>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Jump.performed += JumpInput;

        if (_groundCheck == null) Debug.LogError("[PlayerController] La refer�ncia a Ground Check �s null");
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Jump.performed -= JumpInput;
    }

    void Update()
    {
        HandleInputs();
        _timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
        HandleEnvironment();
        Jump();
        ApplyMovement();
    }
    #endregion

    /// <summary>
    /// Function that gets the player movement inputs
    /// </summary>
    private void HandleInputs()
    {
        if (_isOverride) return;

        if (!_playerCombat.DodgeStance && !_playerCombat.IsAttacking)
            _movementInput = _playerInputActions.Player.Move.ReadValue<float>();
        else
            _movementInput = 0;

        _jumpHold = _playerInputActions.Player.Jump.ReadValue<float>() > 0;
    }

    /// <summary>
    /// Function that handles with the external conditions that affect the player movement
    /// </summary>
    private void HandleEnvironment()
    {
        if (!Physics2D.OverlapCircle(_groundCheck.transform.position, 0.1f, LayerMask.GetMask("Ground")) && _isGrounded)
        {
            _coyoteStart = _timer;
            _availableJumps -= 1;
        }

        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, 0.1f, LayerMask.GetMask("Ground"));

        _myAnimator.SetBool("isGrounded", _isGrounded);

        // Raycast to check if the head is colliding with an obstacle
        RaycastHit2D rightRaycast = Physics2D.Raycast(transform.position + new Vector3(0.1f, 0.5f), Vector2.up, 0.5f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position + new Vector3(0.1f, 0.5f), Vector2.up * 0.5f, Color.green);
        RaycastHit2D centerRaycast = Physics2D.Raycast(transform.position + new Vector3(-0.1f, 0.5f), Vector2.up, 0.5f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position + new Vector3(-0.1f, 0.5f), Vector2.up * 0.5f );
        RaycastHit2D leftRaycast = Physics2D.Raycast(transform.position + new Vector3(-0.3f, 0.5f), Vector2.up, 0.5f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position + new Vector3(-0.3f, 0.5f), Vector2.up * 0.5f, Color.red);

        // Moves the player if its partially colliding
        if (rightRaycast && !centerRaycast)
        {
            transform.position -= new Vector3 (0.2f, 0);
        }     
        else if (leftRaycast && !centerRaycast)
        {
            transform.position += new Vector3(0.2f, 0);
        }

        if (centerRaycast) _stopJump = true;
    }

    /// <summary>
    /// Function that handles the horizontal movement of the player
    /// </summary>
    public void HorizontalMovement()
    {
        if (_movementInput == 0)
        {
            _desiredVelocity.x = Mathf.MoveTowards(_desiredVelocity.x, 0, _acceleration * Time.fixedDeltaTime);
        } 
        else
        {
            float acceleration = _isGrounded ? _acceleration : _airAcceleration; 
            _desiredVelocity.x = _desiredVelocity.x = Mathf.MoveTowards(_desiredVelocity.x, _movementInput * _maxSpeed, acceleration * Time.fixedDeltaTime);
        }

        if (_desiredVelocity.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        } 
        else if (_desiredVelocity.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        _myAnimator.SetFloat("horizontalVelocity", Mathf.Abs(_desiredVelocity.x));
    }

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

        if (!_isGrounded && !_jumpHold) _stopJump = true;

        bool bufferedJump = _timer - _jumpPressed <= _bufferTime;
        bool canJump = _availableJumps > 0 || _timer - _coyoteStart <= _coyoteTime;

        if (bufferedJump && canJump && _desiredJump)
        {
            _desiredVelocity.y += _jumpForce;
            _desiredJump = false;
        }

        if (_isGrounded && _desiredVelocity.y <= 0)
        {
            _desiredVelocity.y = -0.1f;
        }
        else
        {
            if (_stopJump) _desiredVelocity.y = Mathf.Min(0, _desiredVelocity.y);
            _desiredVelocity.y = Mathf.MoveTowards(_desiredVelocity.y, -_maxFallSpeed, _fallSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Function that applies the final movement to the player
    /// </summary>
    public void ApplyMovement()
    {
        if (_isOverride) return;
        _rigidbody2D.velocity = _desiredVelocity;
    }

    /// <summary>
    /// Function that executes then the jump button is pressed and lets the script know
    /// </summary>
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            _jumpPressed = _timer;
            _desiredJump = true;
        }
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

    public Rigidbody2D Rigidbody => _rigidbody2D;

    public bool IsOverride { get { return _isOverride; } set { _isOverride = value; } }
    #endregion
}
