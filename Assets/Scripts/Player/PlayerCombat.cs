using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] _attackAreas;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _feet;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Collider2D _hitbox;

    [Header("Attack settings")]
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackCd;
    [SerializeField] private float _maxTimeBetweenAttacks;
    [SerializeField] private int _maxComboLength;
    [SerializeField] private float _fallExplosionRange;
    [SerializeField] private float _fallExplosionDamage;
    [SerializeField] private float _fallExplosionPushForce;
    [SerializeField] private float _defaultComboTime;

    [Header("Parry")]
    [SerializeField] private float _counterDamage;
    [SerializeField] private float _counterRadius;
    [SerializeField] private float _parryCd;

    [Header("Hit")]
    [SerializeField] private float _horizontalForce;
    [SerializeField] private float _verticalForce;

    [Header("Audio")]
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _parrySound;
    [SerializeField] private AudioClip _counterSound;

    [Header("Audio Volumes")]
    [SerializeField] private float _hitVolume;
    [SerializeField] private float _damageVolume;
    [SerializeField] private float _parryVolume;
    [SerializeField] private float _counterVolume;

    private PlayerAttackComponent[] _attackComponents;
    private Queue<AttackTypes> _attackBuffer;
    private bool _isAttacking;
    private float _attackCdTimer;
    private float _attackPerformed;
    private float _timer;
    private bool _isComboAnimation;
    private bool _isCombo;
    private bool _isParrying;
    private bool _deflect;
    private float _comboTime;

    private Animator _myAnimator;
    private AudioSource _audioSource;
    private bool _isInvulnerable;
    private int _currComboLength;
    private float _parryPerformedTime;

    public bool ComboIsExtended { get; set; }
    public enum DodgeType { HighDodge, LowDodge }
    public enum AttackTypes { LightAttack, HeavyAttack }

    /// <summary>
    /// Player combo states to keep track of the current attack playing. The f for fast and s for slow
    /// </summary>
    public enum ComboStates { Idle, F, Ff, Fff, S, Sf}

    private ComboStates _comboState;
    private DodgeType _dodgeType;

    #endregion

    #region Unity methods
    private void Awake()
    {
        _attackBuffer = new Queue<AttackTypes>();
        _attackComponents = new PlayerAttackComponent[3];
        _audioSource = GetComponent<AudioSource>();
        _myAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        int i = 0;
        foreach(GameObject attackArea in _attackAreas)
        {
            attackArea.SetActive(false);
            _attackComponents[i] = attackArea.GetComponent<PlayerAttackComponent>();
            i++;
        }
        _comboState = ComboStates.Idle;
        _comboTime = _defaultComboTime;
    }

    void Update()
    {
        HandleTimers();
        ExecuteAttack();
    }
    #endregion

    /// <summary>
    /// Controls the attack and dodge timers
    /// </summary>
    private void HandleTimers()
    {
        if (!_isAttacking) _attackCdTimer += Time.deltaTime;
        _timer += Time.deltaTime;
    }

    #region Attack
    /// <summary>
    /// Method called when the fastAttack button is pressed
    /// </summary>
    public void HandleFastAttackInput()
    {
        //Only attacks if the player is not dodging or it is not in coolDown
        if (_attackCdTimer > _attackCd && !_playerController.IsDashing)
        {
            if (_isAttacking) _attackBuffer.Clear();
            _attackBuffer.Enqueue(AttackTypes.LightAttack);
        }
    }

    /// <summary>
    /// Method called when the slowAttack button is pressed
    /// </summary>
    public void HandleSlowAttackInput()
    {
        //Only attacks if the player is not dodging or it is not in coolDown
        if (_attackCdTimer > _attackCd && !_playerController.IsDashing)
        {
            if (_isAttacking) _attackBuffer.Clear();
            _attackBuffer.Enqueue(AttackTypes.HeavyAttack);
        }
    }

    public void ResetAttackCd()
    {
        _attackCdTimer = 5;
    }

    public void HandleThrowBottleInput()
    {
        if (!_isComboAnimation)
        {
            if (_playerController.TryGetItem(InventoryItem.ItemType.Bottle, out InventoryItem bottleData))
            {
                Vector2 direction = gameObject.transform.right;
                Bottle bottle = (Bottle)bottleData;
                bottle.Object.GetComponent<BottleScript>().Throw(direction);
            }
        }
    }

    public void CancelCombo()
    {
        _myAnimator.SetBool("isCombo", false);
        _isComboAnimation = false;
        
        _comboTime = _defaultComboTime;
        _isCombo = false;
        _currComboLength = 0;
        ComboIsExtended = false;
        _isAttacking = false;

        _attackBuffer.Clear();

        EndCombo();


        _playerController.IsOverride = false;
    }

    /// <summary>
    /// Executes the next attack stored in the input buffer
    /// </summary>
    private void ExecuteAttack()
    {
        //Debug.Log(_currComboLength);

        if (_isAttacking) return;

        if (_timer - _attackPerformed > _maxTimeBetweenAttacks && _isComboAnimation)
        {
            _myAnimator.SetBool("isCombo", false);
            _isComboAnimation = false;
            _comboState = ComboStates.Idle;
            _attackCdTimer = 0;
        }

        if (_timer - _attackPerformed > _comboTime && _isCombo)
        {
            _comboTime = _defaultComboTime;
            _isCombo = false;
            _currComboLength = 0;
            ComboIsExtended = false;
        }

        if (_attackBuffer.TryDequeue(out AttackTypes attack))
        {
            if (_currComboLength == 0)
            {
                UIController.Instance.ShowCurrentCombo(_currComboLength);
            }

            _audioSource.PlayOneShot(_hitSound, _hitVolume);
            _isCombo = true;
            _isAttacking = true;
            _myAnimator.SetBool("isCombo", true);
            _currComboLength++;

            UIController.Instance.ShowCurrentCombo(_currComboLength, attack);

            HandleCombos(attack);
        }
    }

    public void ExtendComboTime(float time)
    {
        if (ComboIsExtended) return;

        ComboIsExtended = true;
        _comboTime += time;
    }

    private void HandleCombos(AttackTypes attack)
    {
        _isComboAnimation = true;

        if (_playerController.IsGrounded)
        {
            switch (_comboState)
            {
                case ComboStates.Idle:
                    if (attack == AttackTypes.LightAttack)
                    {
                        _myAnimator.SetTrigger("FastAttack");
                        _comboState = ComboStates.F;
                    }
                    else if (attack == AttackTypes.HeavyAttack)
                    {
                        _myAnimator.SetTrigger("SlowAttack");
                        _comboState = ComboStates.S;
                    }
                    break;

                case ComboStates.F:
                    if (attack == AttackTypes.LightAttack)
                    {
                        _myAnimator.SetTrigger("FastAttack");
                        _comboState = ComboStates.Ff;
                    }
                    else if (attack == AttackTypes.HeavyAttack)
                    {
                        _myAnimator.SetTrigger("SlowAttack");
                        _comboState = ComboStates.Idle;
                    }
                    break;

                case ComboStates.Ff:
                    if (attack == AttackTypes.LightAttack)
                    {
                        _myAnimator.SetTrigger("FastAttack");
                        _isComboAnimation = false;
                    }
                    else if (attack == AttackTypes.HeavyAttack)
                    {
                        _myAnimator.SetTrigger("SlowAttack");
                        _isComboAnimation = false;
                    }
                    break;

                case ComboStates.S:
                    if (attack == AttackTypes.LightAttack)
                    {
                        _myAnimator.SetTrigger("FastAttack");
                        _comboState = ComboStates.Sf;
                    }
                    else if (attack == AttackTypes.HeavyAttack)
                    {
                        _myAnimator.SetTrigger("SlowAttack");
                        _isComboAnimation = false;
                    }
                    break;

                case ComboStates.Sf:
                    if (attack == AttackTypes.LightAttack)
                    {
                        _isAttacking = false;
                        EndCombo();
                    }
                    else if (attack == AttackTypes.HeavyAttack)
                    {
                        _myAnimator.SetTrigger("SlowAttack");
                        _isComboAnimation = false;
                    }
                    break;

            }
        }
        else
        {
            switch (_comboState)
            {
                case ComboStates.Idle:
                    if (attack == AttackTypes.LightAttack)
                    {
                        _myAnimator.SetTrigger("FastAttack");
                        _comboState = ComboStates.F;
                    }
                    else if (attack == AttackTypes.HeavyAttack)
                    {
                        _myAnimator.SetTrigger("SlowAttack");
                        _comboTime = _defaultComboTime;
                        _isComboAnimation = false;
                        MovingDownAttackStart(50);
                    }
                    break;

                case ComboStates.F:
                    if (attack == AttackTypes.LightAttack)
                    {
                        _myAnimator.SetTrigger("FastAttack");
                        _comboState = ComboStates.Ff;
                    }
                    else if (attack == AttackTypes.HeavyAttack)
                    {
                        _myAnimator.SetTrigger("SlowAttack");
                        _comboTime = _defaultComboTime;
                        _isComboAnimation = false;
                        MovingDownAttackStart(50);
                    }
                    break;

                case ComboStates.Ff:
                    if (attack == AttackTypes.LightAttack)
                    {
                        _myAnimator.SetTrigger("FastAttack");
                        _comboState = ComboStates.Fff;
                    }
                    else if (attack == AttackTypes.HeavyAttack)
                    {
                        _myAnimator.SetTrigger("SlowAttack");
                        _comboTime = _defaultComboTime;
                        _isComboAnimation = false;
                        MovingDownAttackStart(50);
                    }
                    break;

                case ComboStates.Fff:
                    if (attack == AttackTypes.LightAttack)
                    {
                        _isAttacking = false;
                        EndCombo();
                    }
                    else if (attack == AttackTypes.HeavyAttack)
                    {
                        _myAnimator.SetTrigger("SlowAttack");
                        _comboTime = _defaultComboTime;
                        _isComboAnimation = false;
                        MovingDownAttackStart(50);
                    }
                    break;
            }
        }
    }

    public void EndDownAttack()
    {
        _attackAreas[2].SetActive(false);
        AttackFinished(1);

        if (_playerController.IsGrounded)
        {
            Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(_feet.transform.position, _fallExplosionRange, LayerMask.GetMask("Ignore Raycast"));

            foreach (Collider2D collider in enemiesColliders)
            {
                if (collider.TryGetComponent<LifeComponent>(out var life))
                {
                    life.ReceiveHit(_fallExplosionDamage);
                    /*
                    float force = collider.transform.position.x > gameObject.transform.position.x ? _fallExplosionPushForce : -_fallExplosionPushForce;
                    life.SendFlyingOutwards(force);
                    */
                }
            }
        }
    }

    private void EndCombo()
    {
        _comboState = ComboStates.Idle;
        _attackCdTimer = 0;
        _myAnimator.SetBool("isCombo", false);
        _attackBuffer.Clear();
    }

    private void SendEnemiesFly()
    {
        foreach (PlayerAttackComponent attack in _attackComponents)
        {
            attack.AttackType = PlayerAttackComponent.PlayerAttackTypes.upwardsForceAttack;
        }
    }

    private void SendEnemiesDown()
    {
        foreach (PlayerAttackComponent attack in _attackComponents)
        {
            attack.AttackType = PlayerAttackComponent.PlayerAttackTypes.downwardsForceAttack;
        }
    }

    /// <summary>
    /// Enables the attack area (animator method)
    /// </summary>
    private void EnableAttackArea(int type)
    {
        AttackTypes attackType = (AttackTypes)type;

        switch (attackType)
        {
            case AttackTypes.LightAttack:
                _attackAreas[0].SetActive(true);
                break;
            case AttackTypes.HeavyAttack:
                _attackAreas[1].SetActive(true);
                break;
        }
    }

    public void UpwardsAttack()
    {
        _playerController.IsOverride = true;
        _playerController.AddDesiredVelocity(new Vector2(0, 26));
    }

    public void DownwardsAttack()
    {
        _playerController.AddDesiredVelocity(new Vector2(0, -30));
    }

    public void UnstopabbleAttackBegin()
    {
        _hitbox.enabled = false;
        _playerController.IsOverride = true;
    }

    public void UnstopabbleAttackEnd()
    {
        _hitbox.enabled = true;
        _playerController.IsOverride = false;
    }

    public void MovingSideAttackStart(int velocity)
    {
        _playerController.IsOverride = true;
        float direction = PlayerInputsManager.Instance.ReadHorizontalInput();
        if (direction == 0) direction = transform.right.x;
        _playerController.AddDesiredVelocity(new Vector2(50, 0) * direction);
    }

    public void MovingDownAttackStart(int velocity)
    {
        _attackAreas[2].SetActive(true);
        _playerController.IsOverride = true;
        _playerController.Rigidbody.velocity = new Vector2(0, -300);
    }

    /// <summary>
    /// Disables the attack area (animator method)
    /// </summary>
    private void DisableAttackArea(int type)
    {
        AttackTypes attackType = (AttackTypes)type;

        switch (attackType)
        {
            case AttackTypes.LightAttack:
                _attackAreas[0].SetActive(false);
                break;
            case AttackTypes.HeavyAttack:
                _attackAreas[1].SetActive(false);
                break;
        }
    }

    public void DisableAttackAreas()
    {
        foreach (var area in _attackAreas)
        {
            area.SetActive(false);
        }
    }

    /// <summary>
    /// Method called when the attack animation is over (animator method)
    /// </summary>
    public void AttackFinished(int isMoving)
    {
        foreach (PlayerAttackComponent attack in _attackComponents)
        {
            attack.AttackType = PlayerAttackComponent.PlayerAttackTypes.defaultAttack;
        }

        if (isMoving == 1)
            _playerController.IsOverride = false;

        if (!_isComboAnimation)
            EndCombo();

        _attackPerformed = _timer;
        _isAttacking = false;          
    }
    #endregion

    #region Parry
    public void HandleParryInput()
    {
        if (_isAttacking || _playerController.IsDashing || !_playerController.IsGrounded || (_timer - _parryPerformedTime) < _parryCd) return;

        _parryPerformedTime = _timer;
        _myAnimator.SetTrigger("Parry");
        _isParrying = true;
    }

    public void OnDeflect()
    {
        _myAnimator.SetTrigger("Counter");
        _audioSource.PlayOneShot(_parrySound, _parryVolume);
        _hitbox.enabled = false;
        GameController.Instance.StopTime(0f, 0.3f);
    }

    private void PerformCounterAttack()
    {
        _audioSource.PlayOneShot(_counterSound, _counterVolume);
        Collider2D[] enemiesCollider = Physics2D.OverlapCircleAll(transform.position, _counterRadius, LayerMask.GetMask("Enemies"));
        
        foreach (Collider2D enemy in enemiesCollider)
        {
            if (!enemy.CompareTag("Enemy Hitbox")) continue;

            enemy.GetComponent<LifeComponent>().ReceiveHit(_counterDamage);
            GameController.Instance.AddScore(50);
        }     
    }

    private void EnableDeflect()
    {
        _deflect = true;
    }

    private void DisableDeflect()
    {
        _deflect = false;
    }
    #endregion

    #region Hit
    public void GetHit(Vector3 damagePosition)
    {
        CancelCombo();

        _isInvulnerable = true;
        _hitbox.enabled = false;

        _audioSource.PlayOneShot(_damageSound, _damageVolume);

        Vector2 force = new Vector2(_horizontalForce, _verticalForce);
        force.x *= damagePosition.x - transform.position.x > 0 ? -1 : 1;

        _playerController.GetHit(force);

        StartCoroutine(HitVisualFeedback());
    }

    private IEnumerator HitVisualFeedback()
    {
        for (int i = 0; i < 6; i++)
        {
            Color initialColor = _sprite.material.color;
            Color finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

            float elapsedTime = 0f;
            float fadeDuration = 0.25f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                _sprite.material.color = Color.Lerp(initialColor, finalColor, elapsedTime / fadeDuration);
                yield return null;
            }

            initialColor = _sprite.material.color;
            finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
            elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                _sprite.material.color = Color.Lerp(initialColor, finalColor, elapsedTime / fadeDuration);
                yield return null;
            }
        }

        _isInvulnerable = false;
        _hitbox.enabled = true;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (_feet == null) return;

        Gizmos.DrawWireSphere(_feet.transform.position, _fallExplosionRange);
        Gizmos.DrawWireSphere(transform.position, _counterRadius);
    }

    #region Getters
    /// <summary>
    /// Returns the dodge type
    /// </summary>
    public DodgeType GetDodgeType => _dodgeType;

    /// <summary>
    /// Returns wheter the player is attacking or not
    /// </summary>
    public bool IsAttacking => _isAttacking;

    /// <summary>
    /// Returns wheter the player is doing a combo or not
    /// </summary>
    public bool IsCombo => _isComboAnimation;
    public bool IsInvulnerable => _isInvulnerable;
    public int CurrComboLength => _currComboLength;
    public bool IsParrying { get { return _isParrying; } set { _isParrying = value; } }
    public bool Deflect { get { return _deflect; } set { _deflect = value; } }
    public Collider2D Hitbox => _hitbox;
    #endregion

}