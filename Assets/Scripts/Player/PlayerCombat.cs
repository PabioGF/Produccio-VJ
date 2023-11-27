using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _fastAttackArea;
    [SerializeField] private GameObject _slowAttackArea;

    [Header("Attack settings")]
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackCd;
    [SerializeField] private float _maxTimeBetweenAttacks;
    [SerializeField] private int _maxComboLength;

    [Header("Dodge settings")]
    [SerializeField] private float _dodgeDuration;
    [SerializeField] private float _dodgeCd;

    private PlayerInputActions _playerInputActions;
    private Queue<AttackTypes> _attackBuffer;
    private bool _isAttacking;
    private float _attackCdTimer;
    private int _currComboLength;
    private float _attackPerformed;
    private float _timer;

    private bool _dodgeStance;
    private bool _isDodging;
    private float _dodgeCdTimer;
    private Animator _myAnimator;

    public enum DodgeType { HighDodge, LowDodge };
    public enum AttackTypes { FastAttack, SlowAttack }
    private DodgeType _dodgeType;

    #endregion

    #region Unity methods
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.FastAttack.performed += FastAttackInput;
        _playerInputActions.Player.SlowAttack.performed += SlowAttackInput;
        _myAnimator = GetComponent<Animator>();
        _attackBuffer = new Queue<AttackTypes>();

        if (_fastAttackArea == null)
            Debug.LogError("[PlayerAttack] Fast Attack Area reference is null");

        if (_slowAttackArea == null)
            Debug.LogError("[PlayerAttack] Slow Attack Area reference is null");
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.FastAttack.performed -= FastAttackInput;
        _playerInputActions.Player.SlowAttack.performed -= SlowAttackInput;
    }

    void Start()
    {
        _fastAttackArea.SetActive(false);
        _slowAttackArea.SetActive(false);
    }

    void Update()
    {
        HandleTimers();
        HandleDodge();
        ExecuteAttack();
    }
    #endregion

    /// <summary>
    /// Controls the attack and dodge timers
    /// </summary>
    private void HandleTimers()
    {
        if (!_isAttacking) _attackCdTimer += Time.deltaTime;
        if (!_isDodging) _dodgeCdTimer += Time.deltaTime;
        _timer += Time.deltaTime;
    }

    #region Attack
    /// <summary>
    /// Method called when the fastAttack button is pressed
    /// </summary>
    public void FastAttackInput(InputAction.CallbackContext context)
    {
        //Only attacks if the player is not dodging or it is not in coolDown
        if (context.performed && !_dodgeStance && !_isDodging && _attackCdTimer > _attackCd)
        {
            if (_isAttacking) _attackBuffer.Clear();
            _attackBuffer.Enqueue(AttackTypes.FastAttack);
        }
    }

    /// <summary>
    /// Method called when the slowAttack button is pressed
    /// </summary>
    public void SlowAttackInput(InputAction.CallbackContext context)
    {
        //Only attacks if the player is not dodging or it is not in coolDown
        if (context.performed && !_dodgeStance && !_isDodging && _attackCdTimer > _attackCd)
        {
            if (_isAttacking) _attackBuffer.Clear();
            _attackBuffer.Enqueue(AttackTypes.SlowAttack);
        }
    }

    /// <summary>
    /// Executes the next attack stored in the input buffer
    /// </summary>
    private void ExecuteAttack()
    {
        if (_isAttacking) return;

        if (_timer - _attackPerformed > _maxTimeBetweenAttacks)
            _currComboLength = 0;

        if (_attackBuffer.TryDequeue(out AttackTypes attack))
        {
            _currComboLength += 1;
            print(_currComboLength);

            if (_currComboLength > _maxComboLength)
            {
                _attackBuffer.Clear();
                _attackCdTimer = 0;
                _currComboLength = 0;
                return;
            }

            _isAttacking = true;
            _attackPerformed = _timer;

            switch (attack)
            {
                case AttackTypes.FastAttack:
                    _attackBuffer.Clear();
                    _myAnimator.SetTrigger("FastAttack");
                    break;

                case AttackTypes.SlowAttack:
                    _attackBuffer.Clear();
                    _myAnimator.SetTrigger("SlowAttack");
                    break;
            }
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
            case AttackTypes.FastAttack:
                _fastAttackArea.SetActive(true);
                break;
            case AttackTypes.SlowAttack:
                _slowAttackArea.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Disables the attack area (animator method)
    /// </summary>
    private void DisableAttackArea(int type)
    {
        AttackTypes attackType = (AttackTypes)type;

        switch (attackType)
        {
            case AttackTypes.FastAttack:
                _fastAttackArea.SetActive(false);
                break;
            case AttackTypes.SlowAttack:
                _slowAttackArea.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// Method called when the attack animation is over (animator method)
    /// </summary>
    private void AttackFinished()
    {
        _isAttacking = false;
    }
    #endregion

    #region Dodge
    /// <summary>
    /// Handles the dodge logic when holding the dodge trigger buttond and moving to the upper or lower direction axis
    /// </summary>
    private void HandleDodge()
    {
        if (_playerInputActions.Player.DodgeTrigger.ReadValue<float>() == 1) _dodgeStance = true;
        else _dodgeStance = false;

        if (_dodgeStance)
        {
            float dodgeDirection = _playerInputActions.Player.DodgeInput.ReadValue<float>();

            if (_dodgeCdTimer > _dodgeCd)
            {
                if (dodgeDirection == 1)
                {
                    ExecuteDodge(DodgeType.HighDodge);
                }
                if (dodgeDirection == -1)
                {
                    ExecuteDodge(DodgeType.LowDodge);
                }
            }
        }
    }
    
    /// <summary>
    /// Executes the dodge action
    /// </summary>
    /// <param name="dodgeType">The dodge type</param>
    private void ExecuteDodge(DodgeType dodgeType)
    {
        Debug.Log("Dodging");
        _dodgeCdTimer = 0;
        _isDodging = true;
        _dodgeType = dodgeType;
        Invoke(nameof(StopDodge), _dodgeDuration);
    }

    /// <summary>
    /// Stops the dodge action
    /// </summary>
    public void StopDodge()
    {
        _isDodging = false;
    }

    /// <summary>
    /// Method called when the player successfully dodges an attack
    /// </summary>
    public void OnDodge()
    {
        Debug.Log("Dodged");
    }
    #endregion

    #region Getters
    /// <summary>
    /// Returns the dodgeStance bool
    /// </summary>
    public bool DodgeStance => _dodgeStance;

    /// <summary>
    /// Returns the dodge type
    /// </summary>
    public DodgeType GetDodgeType => _dodgeType;

    /// <summary>
    /// Returns wheter the player is dodging or not
    /// </summary>
    public bool IsDodging => _isDodging;
    #endregion

}