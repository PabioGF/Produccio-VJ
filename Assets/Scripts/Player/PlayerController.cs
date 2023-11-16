using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject _groundCheck;
    [SerializeField] GameObject _topCheck;

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

    private PlayerCombat _playerCombat;
    private PlayerInputActions _playerInputActions;
    private Rigidbody2D _rigidbody2D;
    private int _availableJumps;
    private float _movementInput;
    private bool _desiredJump;
    private Vector2 _desiredVelocity;
    private bool _isGrounded;
    private float _jumpPressed;
    private float _timer;
    private bool _stopJump;

    private Animator _myAnimator;
    private bool _hasKey;
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

        if (_groundCheck == null)
        {
            Debug.LogError("[PlayerController] La referència a Ground Check és null");
        }
        if (_topCheck == null)
        {
            Debug.LogError("[PlayerController] La referència a Top Check és null");
        }
        

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
        if (!_playerCombat.DodgeStance())
        {
            _movementInput = _playerInputActions.Player.Move.ReadValue<float>();
        } else
        {
            _movementInput = 0;
        }
    }

    /// <summary>
    /// Function that handles with the external conditions that affect the player movement
    /// </summary>
    private void HandleEnvironment()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, 0.1f, LayerMask.GetMask("Ground"));
        _stopJump = Physics2D.OverlapCircle(_topCheck.transform.position, 0.1f, LayerMask.GetMask("Ground"));

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

        _myAnimator.SetFloat("horizontalVelocity", Mathf.Abs(_desiredVelocity.x));
    }

    /// <summary>
    /// Function that handles the executrion of the jump considering all the variables that are implied
    /// </summary>
    public void Jump()
    {
        if (_isGrounded)
        {
            _availableJumps = _maxJumps;
        }

        bool bufferedJump = _timer - _jumpPressed <= _bufferTime;

        if (_desiredJump && _availableJumps > 0 && bufferedJump)
        {
            _desiredVelocity.y += _jumpForce;
            _availableJumps -= 1;
            _desiredJump = false;
        }

        _myAnimator.SetBool("isGrounded", _isGrounded);
    }

    /// <summary>
    /// Function that applies the final movement to the player
    /// </summary>
    public void ApplyMovement()
    {
        _rigidbody2D.velocity = _desiredVelocity;
    }

    /// <summary>
    /// Function that executes then the jump button is pressed and lets the script know
    /// </summary>
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            _desiredJump = true;
            _jumpPressed = _timer;
        }
    }

    public void GiveKey(bool key)
    {
        _hasKey = key;
    }

    public bool HasKey()
    {
        return _hasKey;
    }
}
