using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    #region Variables
    [Header("Attack settings")]
    [SerializeField] private GameObject _attackArea;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackCd;

    [Header("Dodge settings")]
    [SerializeField] private GameObject _hurtbox;
    [SerializeField] private float _dodgeDuration;
    [SerializeField] private float _dodgeCd;

    private PlayerInputActions _playerInputActions;
    private Queue<AttackTypes> _attackBuffer;
    private bool _isAttacking;
    private float _attackCdTimer;

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

        if (_attackArea == null)
        {
            Debug.LogError("[PlayerAttack] La referència a Attack Area és null");
        }
        if (_hurtbox == null)
        {
            Debug.LogError("[PlayerAttack] La referència a Hurtbox és null");
        }
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.FastAttack.performed -= FastAttackInput;
        _playerInputActions.Player.SlowAttack.performed -= SlowAttackInput;
    }

    void Start()
    {
        _attackArea.SetActive(false);
    }

    void Update()
    {
        HandleTimers();
        HandleDodge();
    }
    #endregion

    /// <summary>
    /// Controls the attack and dodge timers
    /// </summary>
    private void HandleTimers()
    {
        if (!_isAttacking) _attackCdTimer += Time.deltaTime;
        if (!_isDodging) _dodgeCdTimer += Time.deltaTime;
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
            Debug.Log("FastAttack");
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
            Debug.Log("SlowAttack");
            _attackBuffer.Enqueue(AttackTypes.SlowAttack);
        }
    }

    private void ExecuteAttack()
    {
        if (_attackBuffer.TryDequeue(out AttackTypes attack))
        {
            _isAttacking = true;
            _myAnimator.SetTrigger("startAttack");
            _attackCdTimer = 0;

            if (attack == AttackTypes.FastAttack)
            {

            }
            else if (attack == AttackTypes.SlowAttack)
            {

            } 
        }
    }

    /// <summary>
    /// Enables the attack area (animator method)
    /// </summary>
    private void EnableAttackArea()
    {
        _attackArea.SetActive(true);
    }

    /// <summary>
    /// Disables the attack area (animator method)
    /// </summary>
    private void DisableAttackArea()
    {
        _attackArea.SetActive(false);
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