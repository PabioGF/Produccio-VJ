using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    #region Global Variables
    public float rightPositionLimit;
    public float leftPositionLimit;

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _groundCheck;

    [Header("Cooldowns")]
    [SerializeField] private float _meleeAttackCd;
    [SerializeField] private float _rangedAttackCd;
    [SerializeField] private float _jumpCd;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private bool _isFlipped;
    private bool _canAttack;
    private bool _canShoot;
    private bool _canJump;
    private bool _isGrounded;
    #endregion

    #region Unity Methods
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _canAttack = true;
        _canShoot = true;
        _canJump = true;
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, 0.1f, LayerMask.GetMask("Ground"));
        _animator.SetBool("IsGrounded", _isGrounded);
        if (_isGrounded )
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
    #endregion

    /// <summary>
    /// Rotates the boss so it always looks at the player
    /// </summary>
    public void LookAtPlayer()
    {
        if (transform.position.x > _player.position.x && !_isFlipped)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            _isFlipped = true;
        }
        else if (transform.position.x < _player.position.x && _isFlipped)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _isFlipped = false;
        }
    }

    #region Melee Attacks
    /// <summary>
    /// Triggers the melee attack animation
    /// </summary>
    public void Attack()
    {
        Debug.Log("Attack");
        _animator.SetTrigger("Attack");
        _canAttack = false;
        Invoke(nameof(ResetMeleeAttack), _meleeAttackCd);
    }

    /// <summary>
    /// Enables the boss to perform the melee attack again
    /// </summary>
    private void ResetMeleeAttack()
    {
        _canAttack = true;
    }
    #endregion

    #region Ranged Attacks
    /// <summary>
    /// Triggers the ranged attack animation
    /// </summary>
    public void Shoot()
    {
        Debug.Log("Shoot");
        _animator.SetTrigger("Shoot");
        _canShoot = false;
        Invoke(nameof(ResetRangedAttack), _rangedAttackCd);
    }
   
    /// <summary>
    /// Enables the boss to perform the ranged attack again
    /// </summary>
    private void ResetRangedAttack()
    {
        _canShoot = true;
    }
    #endregion

    #region Jump
    /// <summary>
    /// Triggers the jump animation and goes up
    /// </summary>
    public void Jump()
    {
        Debug.Log("Jump");
        _canJump = false;
        _rigidbody.velocity = new Vector2(0, 50);
    }

    /// <summary>
    /// Falls back to the stage
    /// </summary>
    public void Fall()
    {
        if (Random.value >= 0.5)
        {
            Debug.Log("Left");
            _rigidbody.MovePosition(new Vector2(-15, _rigidbody.position.y));
        }
        else
        {
            Debug.Log("Right");
            _rigidbody.MovePosition(new Vector2(20, _rigidbody.position.y));
        }
        _rigidbody.velocity = new Vector2(0, -50);
        Invoke(nameof(ResetJump), _jumpCd);
    }

    /// <summary>
    /// Enables the boss to perform the jump movement again
    /// </summary>
    private void ResetJump()
    {
        _canJump = true;
    }
    #endregion

    #region Die
    /// <summary>
    /// Triggers the death animation
    /// </summary>
    public void Die()
    {
        _animator.SetTrigger("Die");
    }

    /// <summary>
    /// Disables the boss and triggers the needed events
    /// </summary>
    private void DisableBoss()
    {
        gameObject.SetActive(false);
    }

    #endregion

    public bool CanAttack { get { return _canAttack; } }
    public bool CanShoot { get { return _canShoot; } }
    public bool CanJump { get { return _canJump; } }
}