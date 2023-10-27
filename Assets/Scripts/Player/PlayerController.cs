using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    #region Variables
    [SerializeField] GameObject _groundCheck;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _maxJumps;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _bufferTime;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _maxFallSpeed;

    private PlayerInputActions _playerInputActions;
    private Rigidbody2D _rigidbody2D;
    private int _availableJumps;
    private float _movementInput;
    private bool _desiredJump;
    private Vector2 _desiredVelocity;
    private bool _isGrounded;
    private float _jumpPressed;
    private float _timer;
    #endregion

    #region Unity methods
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Jump.performed += JumpInput;

        if (_groundCheck == null)
        {
            Debug.LogError("[PlayerController] La referència a groundCheck és null");
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
        HandleEnvironment();
        Jump();
        ApplyMovement();
    }
    #endregion

    private void HandleInputs()
    {
        _movementInput = _playerInputActions.Player.Move.ReadValue<float>();
    }

    private void HandleEnvironment()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, 0.1f, LayerMask.GetMask("Ground"));

        if (_isGrounded && _desiredVelocity.y <= 0)
        {
            _desiredVelocity.y = -0.1f;
        }
        else
        {
            _desiredVelocity.y = Mathf.MoveTowards(_desiredVelocity.y, -_maxFallSpeed, _fallSpeed * Time.fixedDeltaTime);
        }
    }

    public void HorizontalMovement()
    {
        _desiredVelocity.x = _desiredVelocity.x = Mathf.MoveTowards(_desiredVelocity.x, _movementInput * _maxSpeed, _acceleration * Time.fixedDeltaTime);
    }

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
    }

    public void ApplyMovement()
    {
        _rigidbody2D.velocity = _desiredVelocity;
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        _desiredJump = true;
        _jumpPressed = _timer;
    }
}
