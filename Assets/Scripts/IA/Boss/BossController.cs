using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    #region Global Variables
    public float rightPositionLimit;
    public float leftPositionLimit;

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private GameObject _jumpHitbox;
    [SerializeField] private GameObject[] _specialHitAreas;

    [Header("Melee Attack Parameters")]
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDamage;

    [Header("Ranged Attack parameters")]
    [SerializeField] private GameObject _highBullet;
    [SerializeField] private GameObject _lowBullet;

    [Header("Special Attack Parameters")]
    [SerializeField] private float _minSeparation;
    [SerializeField] private float _maxSeparation;
    [SerializeField] private float _slashInBetweenTime;

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
    private bool _isJumping;

    private bool _hasFallenRight;
    private int _phase;
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
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, 0.2f, LayerMask.GetMask("Ground"));
        _animator.SetBool("IsGrounded", _isGrounded);
        if (_isGrounded && !_isJumping)
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;
        
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
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

    public void EnterSecondPhase()
    {
        _phase = 1;
        _animator.SetInteger("Phase", _phase);
    }

    #region Melee Attacks
    /// <summary>
    /// Triggers the melee attack animation
    /// </summary>
    public void BeginCombo()
    {
        Debug.Log("Attack");
        _animator.SetTrigger("Attack");
        _canAttack = false;
        Invoke(nameof(ResetMeleeAttack), _meleeAttackCd);
    }

    private void PerformAttack(int type)
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, LayerMask.GetMask("PlayerHitbox"));

        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerLifeComponent>().ReceiveHit(_attackDamage, transform.position);
        }
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
    /// Triggers the shoot animation
    /// </summary>
    public void StartShooting()
    {
        _animator.SetTrigger("Shoot");
        _canShoot = false;
    }

    /// <summary>
    /// Shoots a random type of bullet to the player
    /// </summary>
    private void Shoot()
    {
        if (Random.value >= 0.5)
        {
            GameObject bullet = Instantiate(_highBullet, _attackPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().SetDirection(transform.right);
        }
        else
        {
            GameObject bullet = Instantiate(_lowBullet, _attackPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().SetDirection(transform.right);
        }
    }

    /// <summary>
    /// Invokes the ranged attack reset
    /// </summary>
    private void StopShooting()
    {
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
        _isJumping = true;
        _canJump = false;
        _rigidbody.velocity = new Vector2(0, 50);
        _jumpHitbox.SetActive(true);
    }

    /// <summary>
    /// Falls back to the stage
    /// </summary>
    public void Fall()
    {
        _isJumping = false;

        if (Random.value >= 0.5)
        {
            _rigidbody.MovePosition(new Vector2(-15, _rigidbody.position.y));
            _hasFallenRight = false;
        }
        else
        {
            _rigidbody.MovePosition(new Vector2(20, _rigidbody.position.y));
            _hasFallenRight = true;
        }
        _rigidbody.velocity = new Vector2(0, -10);
        LookAtPlayer();

        if (_phase > 0 /*&& Random.value >= 0.3*/) _animator.SetTrigger("Special");

        Invoke(nameof(ResetJump), _jumpCd);
    }

    /// <summary>
    /// Disables the jump hitbox
    /// </summary>
    public void DisableJumpHitbox()
    {
        if (_jumpHitbox.activeSelf)
        {
            _jumpHitbox.SetActive(false);
        }
    }

    /// <summary>
    /// Enables the boss to perform the jump movement again
    /// </summary>
    private void ResetJump()
    {
        _canJump = true;
    }
    #endregion

    #region Special Attack
    private void SpecialAttackSetup()
    {
        float _currentOffset = 0;

        foreach (GameObject area in _specialHitAreas) 
        {
            float separation = Random.Range(_minSeparation, _maxSeparation);
            if (_hasFallenRight) separation *= -1;

            Vector2 newPos = transform.position;
            newPos.y += 5;
            newPos.x += separation + _currentOffset;
            area.transform.position = newPos;

            if (Random.value >= 0.5) area.transform.eulerAngles = new Vector3(0, 0, 180);
            else area.transform.eulerAngles = new Vector3(0, 0, 0);

            _currentOffset += separation;

            Debug.Log("Separation: " + separation);
            Debug.Log("Current offset: " + _currentOffset);
        }
    }

    private void AnimateSlashesTrigger()
    {
        StartCoroutine(AnimateSlashes());
    }

    private IEnumerator AnimateSlashes()
    {
        foreach (GameObject area in _specialHitAreas)
        {
            area.GetComponent<Animator>().SetTrigger("Appear");
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1);

        foreach (GameObject area in _specialHitAreas)
        {
            area.GetComponent<Animator>().SetTrigger("Slash");
            yield return new WaitForSeconds(_slashInBetweenTime);
        }
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