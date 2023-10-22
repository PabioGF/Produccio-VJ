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

    private PlayerInputActions _playerInputActions;
    private Rigidbody2D _rigidbody2D;
    private int _availableJumps;
    private float _movementInput;
    private bool _desiredJump;
    private Vector2 _desiredVelocity;
    #endregion

    #region Unity methods
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _desiredVelocity = Vector2.zero;

        _playerInputActions.Player.Jump.performed += JumpInput;



        if (_groundCheck == null)
        {
            Debug.LogError("[PlayerController] La referència a groundCheck és null");
        }
    }

    void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
        Jump();
        ApplyMovement();
    }
    #endregion

    private void HandleInputs()
    {
        _movementInput = _playerInputActions.Player.Move.ReadValue<float>();
    }

    public void HorizontalMovement()
    {
        _desiredVelocity = _rigidbody2D.velocity;
        _desiredVelocity.x = _movementInput * _speed;
    }

    public void Jump()
    {
        bool isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, 0.1f, LayerMask.GetMask("Ground"));

        if (isGrounded && !Input.GetKey(KeyCode.Space))
        {
            _availableJumps = _maxJumps;
        }

        if (_desiredJump && _availableJumps > 0)
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
    }
}
